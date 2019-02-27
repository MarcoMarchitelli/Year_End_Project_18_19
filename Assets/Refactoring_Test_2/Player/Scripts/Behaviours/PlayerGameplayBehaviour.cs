﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Refactoring
{
    [RequireComponent(typeof(PlayerCollisionsBehaviour))]
    public class PlayerGameplayBehaviour : BaseBehaviour
    {
        PlayerEntityData data;

        #region Serialized Fields

        [Header("Movement")]
        [SerializeField] float moveSpeed = 6;
        [Tooltip("Time it takes to the player to linearly interpolate from no speed to move speed, while grounded")]
        [SerializeField] float accelerationTimeGrounded = 0f;
        [Tooltip("Time it takes to the player to linearly interpolate from no speed to move speed, while in air")]
        [SerializeField] float accelerationTimeAirborne = .2f;

        [Header("Sprinting")]
        [Tooltip("Behaviour's running speed.")]
        [SerializeField] float runMoveSpeed;
        [Tooltip("Time the entity takes to reach the end of the run acceleration curve.")]
        [SerializeField] float accelerationTime;
        [Tooltip("Describes how the move speed interpolates to run speed.")]
        [SerializeField] AnimationCurve runAccelerationCurve;
        [Tooltip("Time the entity takes to reach the end of the run deceleration curve.")]
        [SerializeField] float decelerationTime;
        [Tooltip("Describes how the run speed interpolates back to move speed.")]
        [SerializeField] AnimationCurve runDecelerationCurve;

        [Header("Dashing")]
        [SerializeField] float dashDistance = 5f;
        [SerializeField] float dashDuration;

        [Header("Jumping")]
        public float maxJumpHeight = 4;
        public float minJumpHeight = 1;
        public float timeToJumpApex = .4f;

        #endregion

        #region Vars

        #region Wall Jumping

        [HideInInspector]
        public Vector2 wallJumpClimb;
        [HideInInspector]
        public Vector2 wallJumpOff;
        [HideInInspector]
        public Vector2 wallLeap;
        [HideInInspector]
        public float wallSlideSpeedMax = 3;
        [HideInInspector]
        public float wallStickTime = .25f;
        float timeToWallUnstick;
        bool wallSliding;
        int wallDirX;

        #endregion

        #region Normal Velocity

        float currentMoveSpeed;
        float gravity;
        float maxJumpVelocity;
        float minJumpVelocity;
        Vector3 velocity;
        float velocityXSmoothing;

        #endregion

        #region Sprinting

        bool accelerating;
        float accelTimer;
        bool decelerating;
        float decelTimer;
        bool runReleasedInAir;
        bool groundCollisionDecelHandled;

        #endregion

        #region Dashing

        Vector3 dashDirection;
        float dashSpeed;
        const int airDashes = 1;
        int airDashesCount;
        float dashTimer;
        bool _isDashing = false;
        bool IsDashing
        {
            get { return _isDashing; }
            set
            {
                if (_isDashing != value)
                {
                    _isDashing = value;
                    data.animatorProxy.IsDashing = _isDashing;
                    if (_isDashing)
                    {
                        dashTimer = 0;
                    }
                }
            }
        }

        #endregion

        #region Faceing

        int _faceingDirection;
        int FaceingDirection
        {
            get { return _faceingDirection; }
            set
            {
                _faceingDirection = value;
                if (_faceingDirection > 0)
                    data.animatorProxy.transform.rotation = Quaternion.Euler(rightFaceingDirection);
                else if (_faceingDirection < 0)
                    data.animatorProxy.transform.rotation = Quaternion.Euler(leftFaceingDirection);
            }
        }
        Vector3 rightFaceingDirection = Vector3.zero;
        Vector3 leftFaceingDirection = Vector3.up * 180;

        #endregion

        Vector2 directionalInput;

        #endregion

        protected override void CustomSetup()
        {
            data = Entity.Data as PlayerEntityData;

            gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
            currentMoveSpeed = moveSpeed;
            accelerationTime = Mathf.Abs(accelerationTime);
            decelerationTime = Mathf.Abs(decelerationTime);
            dashSpeed = dashDistance / dashDuration;
            airDashesCount = 0;
        }

        public override void OnUpdate()
        {
            #region Calculations

            HandleSprinting();
            HandleDashing();
            CalculateVelocity();
            //HandleWallSliding();

            #endregion

            #region Movement

            data.controller3D.Move(velocity * Time.deltaTime, directionalInput);

            #endregion

            #region Collisions

            if (data.controller3D.collisions.above || data.controller3D.Below)
            {
                velocity.y = 0;
            }

            #endregion

            #region Animations

            HandleAnimations();

            #endregion

        }

        #region API

        public void SetDirectionalInput(Vector2 input)
        {
            directionalInput = input;
        }

        public void OnJumpInputDown()
        {
            if (wallSliding)
            {
                if (wallDirX == directionalInput.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (directionalInput.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (data.controller3D.Below)
            {
                velocity.y = maxJumpVelocity;
            }
        }

        public void OnJumpInputUp()
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }

        public void HandleDashPress()
        {
            if (IsSetupped && !IsDashing)
            {
                if (data.controller3D.Below)
                    StartDash();
                else if (airDashesCount < 1)
                    StartDash(true);
            }
        }

        public void HandleSprintPress()
        {
            if (data.controller3D.Below)
            {
                accelerating = true;
                decelerating = false;
                decelTimer = 0;
            }
        }

        public void HandleSprintRelease()
        {
            //Reset accel timer.
            accelerating = false;
            accelTimer = 0;

            //check ground collision
            if (data.controller3D.Below)
            {
                decelerating = true;
                runReleasedInAir = false;
            }
            else
            {
                runReleasedInAir = true;
                groundCollisionDecelHandled = false;
            }

        }

        /// <summary>
        /// Checks needed to see wether we have momentum to take in account when touching the ground.
        /// </summary>
        /// <param name="_value"></param>
        public void HandleGroundCollision()
        {
            if (runReleasedInAir && !groundCollisionDecelHandled)
            {
                decelerating = true;
                decelTimer = 0;
                groundCollisionDecelHandled = true;
            }

            airDashesCount = 0;
        }

        #endregion

        #region Player Gameplay Methods

        void StartDash(bool _isAirDash = false)
        {
            if (_isAirDash)
                airDashesCount++;
            IsDashing = true;
        }

        void HandleDashing()
        {
            if (IsDashing)
            {
                dashTimer += Time.deltaTime;
                if (dashTimer >= dashDuration)
                    IsDashing = false;
            }
        }

        void HandleWallSliding()
        {
            wallDirX = (data.controller3D.collisions.left) ? -1 : 1;
            wallSliding = false;
            if ((data.controller3D.collisions.left || data.controller3D.collisions.right) && !data.controller3D.Below && velocity.y < 0)
            {
                wallSliding = true;

                if (velocity.y < -wallSlideSpeedMax)
                {
                    velocity.y = -wallSlideSpeedMax;
                }

                if (timeToWallUnstick > 0)
                {
                    velocityXSmoothing = 0;
                    velocity.x = 0;

                    if (directionalInput.x != wallDirX && directionalInput.x != 0)
                    {
                        timeToWallUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }

            }

        }

        void HandleSprinting()
        {
            if (accelerating)
            {
                accelTimer += Time.deltaTime;
                if (accelTimer > accelerationTime)
                    accelTimer = accelerationTime;
                EvaluateSprintCurve(accelerating);
            }

            if (decelerating)
            {
                decelTimer += Time.deltaTime;
                if (decelTimer > decelerationTime)
                    decelTimer = decelerationTime;
                EvaluateSprintCurve(accelerating);
            }
        }

        float interpolationValue;
        void EvaluateSprintCurve(bool _isAccelerating)
        {
            if (_isAccelerating)
            {
                if (accelerationTime != 0)
                {
                    interpolationValue = runAccelerationCurve.Evaluate(accelTimer / accelerationTime);
                    currentMoveSpeed = Mathf.Lerp(moveSpeed, runMoveSpeed, interpolationValue);
                }
                else
                {
                    currentMoveSpeed = runMoveSpeed;
                }
            }
            else
            {
                if (decelerationTime != 0)
                {
                    interpolationValue = runDecelerationCurve.Evaluate(decelTimer / decelerationTime);
                    currentMoveSpeed = Mathf.Lerp(moveSpeed, runMoveSpeed, interpolationValue);
                }
                else
                {
                    currentMoveSpeed = moveSpeed;
                }
            }
        }

        void CalculateVelocity()
        {
            if (!IsDashing)
            {
                float targetVelocityX = directionalInput.x * currentMoveSpeed;
                velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (data.controller3D.Below) ? accelerationTimeGrounded : accelerationTimeAirborne);
                velocity.y += gravity * Time.deltaTime;
            }
            else
            {
                velocity.x = dashSpeed * FaceingDirection;
                velocity.y = 0;
            }
        }

        void HandleAnimations()
        {
            FaceingDirection = (int)Mathf.Sign(velocity.x);

            if (data.controller3D.Below)
                data.animatorProxy.IsGrounded = true;
            else
                data.animatorProxy.IsGrounded = false;

            if (velocity.y > 0)
                data.animatorProxy.IsRising = true;
            else if (velocity.y < 0)
                data.animatorProxy.IsRising = false;

            if (velocity == Vector3.zero)
                data.animatorProxy.IsWalking = false;
            else
                data.animatorProxy.IsWalking = true;
        }

        #endregion
    }
}