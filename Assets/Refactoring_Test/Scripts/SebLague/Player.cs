using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Refactoring
{
    [RequireComponent(typeof(Controller3D))]
    public class Player : BaseBehaviour
    {
        PlayerEntityData data;

        #region Vars

        public float maxJumpHeight = 4;
        public float minJumpHeight = 1;
        public float timeToJumpApex = .4f;
        [SerializeField]
        float accelerationTimeAirborne = .2f;
        [SerializeField]
        float accelerationTimeGrounded = 0f;
        [SerializeField]
        float moveSpeed = 6;

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
        [HideInInspector]
        float timeToWallUnstick;

        float gravity;
        float maxJumpVelocity;
        float minJumpVelocity;
        Vector3 velocity;
        float velocityXSmoothing;

        Vector2 directionalInput;
        Vector3 rightFaceingDirection = Vector3.zero;
        Vector3 leftFaceingDirection = Vector3.up * 180;
        bool wallSliding;
        int wallDirX;

        #endregion

        protected override void CustomSetup()
        {
            data = Entity.Data as PlayerEntityData;

            gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        }

        public override void OnUpdate()
        {
            CalculateVelocity();
            //HandleWallSliding();

            data.controller3D.Move(velocity * Time.deltaTime, directionalInput);

            if (data.controller3D.collisions.above || data.controller3D.collisions.below)
            {
                velocity.y = 0;
            }

            HandleAnimations();

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
            if (data.controller3D.collisions.below)
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

        #endregion

        #region Player Gameplay Methods

        void HandleWallSliding()
        {
            wallDirX = (data.controller3D.collisions.left) ? -1 : 1;
            wallSliding = false;
            if ((data.controller3D.collisions.left || data.controller3D.collisions.right) && !data.controller3D.collisions.below && velocity.y < 0)
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

        void CalculateVelocity()
        {
            float targetVelocityX = directionalInput.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (data.controller3D.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            velocity.y += gravity * Time.deltaTime;
        }

        void HandleAnimations()
        {
            if (velocity.x > 0)
                data.animatorProxy.transform.rotation = Quaternion.Euler(rightFaceingDirection);
            else if(velocity.x < 0)
                data.animatorProxy.transform.rotation = Quaternion.Euler(leftFaceingDirection);

            if (data.controller3D.collisions.below)
                data.animatorProxy.IsGrounded = true;
            else
                data.animatorProxy.IsGrounded = false;

            if (velocity.y > 0)
                data.animatorProxy.IsRising = true;
            else
                data.animatorProxy.IsRising = false;

            if (velocity == Vector3.zero)
                data.animatorProxy.IsWalking = false;
            else
                data.animatorProxy.IsWalking = true;
        }

        #endregion
    }
}