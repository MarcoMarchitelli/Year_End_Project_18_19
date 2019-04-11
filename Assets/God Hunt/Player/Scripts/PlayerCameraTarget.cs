using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerCameraTarget : MonoBehaviour
{
    #region RaycastController

    public LayerMask collisionMask;

    [SerializeField] BoxCollider collider;

    #region Rays settings
    public const float skinWidth = .015f;
    const float dstBetweenRays = .15f;
    [HideInInspector]
    public int horizontalRayCount;
    [HideInInspector]
    public int verticalRayCount;
    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;
    #endregion

    public RaycastOrigins raycastOrigins;

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    #endregion

    #region 2D Controller

    float maxClimbAngle = 80;
    float maxDescendAngle = 80;
    public CollisionInfo collisions;
    [SerializeField] float traversableTime = .5f;
    [HideInInspector] public bool CollidingWithTraversable = false;

    public Vector2 Move(Vector2 moveAmount, bool standingOnPlatform)
    {
        return Move(moveAmount, Vector2.zero, standingOnPlatform);
    }

    public Vector2 Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();

        collisions.Reset();
        collisions.moveAmountOld = moveAmount;

        if (moveAmount.y < 0)
        {
            DescendSlope(ref moveAmount);
        }

        if (moveAmount.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
        }

        HorizontalCollisions(ref moveAmount);
        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount);
        }

        transform.Translate(moveAmount);

        if (standingOnPlatform)
        {
            collisions.below = true;
        }

        return moveAmount;
    }

    void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float originalMoveAmountX = moveAmount.x;
        Collider otherCollider = null;

        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        if (Mathf.Abs(moveAmount.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit hit;

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (Physics.Raycast(rayOrigin, Vector2.right * directionX, out hit, rayLength, collisionMask))
            {

                if (hit.distance == 0)
                {
                    continue;
                }

                otherCollider = hit.collider;

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        moveAmount = collisions.moveAmountOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref moveAmount, slopeAngle);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }

        //PUSHABLE OBJECT THING
        //if (otherCollider != null && otherCollider.gameObject != this.gameObject && otherCollider.tag == "Pushable")
        //{
        //    Vector2 pushAmount = otherCollider.gameObject.GetComponent<PushableObject>().Push(new Vector2(originalMoveAmountX, 0));
        //    //print (moveAmount.y);
        //    moveAmount = new Vector2(pushAmount.x, moveAmount.y + pushAmount.y);
        //    collisions.left = false;
        //    collisions.right = false;
        //}
    }

    void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {

            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit hit;

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (Physics.Raycast(rayOrigin, Vector2.up * directionY, out hit, rayLength, collisionMask))
            {
                if (hit.collider.tag == "Through")
                {
                    if (directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if (collisions.fallingThroughPlatform)
                    {
                        continue;
                    }
                    if (directionY == -1)
                        CollidingWithTraversable = true;
                }
                else
                {
                    CollidingWithTraversable = false;
                }

                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, Vector2.up * directionY, out hit, rayLength, collisionMask))
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector2 moveAmount, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (moveAmount.y <= climbmoveAmountY)
        {
            moveAmount.y = climbmoveAmountY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector2 moveAmount)
    {
        float directionX = Mathf.Sign(moveAmount.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
                    {
                        float moveDistance = Mathf.Abs(moveAmount.x);
                        float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                        moveAmount.y -= descendmoveAmountY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    public void SetFallingThrowPlatform()
    {
        collisions.fallingThroughPlatform = true;
        Invoke("ResetFallingThroughPlatform", traversableTime);
    }

    void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector2 moveAmountOld;
        public int faceDir;
        public bool fallingThroughPlatform;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

    #endregion

    #region Movement

    public float freeLookMoveSpeed = 3f;
    public float lookAheadX;
    public float speedXMultiplier = 1.2f;
    public float lookAheadY;
    public float speedYMultiplier = 1.2f;
    Vector2 inputDir;
    Vector2 velocity;
    Vector2 oldVelocity;
    bool calculateCollisions;
    bool freeLookMode;
    Vector2 directionToFather;
    float xLimit, yLimit;

    private void Update()
    {
        CalculateData();

        print(velocity);

        if (calculateCollisions)
        {
            Move(velocity * Time.deltaTime, false);

            if (collisions.above || collisions.below)
            {
                velocity.y = 0;
            }
        }
        else
        {
            transform.Translate(velocity * Time.deltaTime);
        }

        HandlePositionX();
        HandlePositionY();
    }

    void HandlePositionX()
    {
        if (Mathf.Abs(transform.position.x) >= xLimit)
            transform.position = new Vector2(xLimit, transform.position.y);
    }

    void HandlePositionY()
    {
        if (Mathf.Abs(transform.position.y) >= yLimit)
            transform.position = new Vector2(transform.position.x, yLimit);
    }

    void CalculateData()
    {
        directionToFather = (transform.parent.position - transform.position).normalized;
        xLimit = transform.parent.position.x + lookAheadX;
        yLimit = transform.parent.position.y + lookAheadY;
    }

    public void SetVelocity(Vector2 _newVelocity)
    {
        //if accelerating or steady speed
        if (_newVelocity.sqrMagnitude >= oldVelocity.sqrMagnitude)
        {
            velocity = new Vector2(_newVelocity.x * speedXMultiplier, _newVelocity.y * speedYMultiplier);
            calculateCollisions = true;
        }
        //if decelerating
        else if (_newVelocity.sqrMagnitude < oldVelocity.sqrMagnitude)
        {
            velocity = directionToFather * 7;
            calculateCollisions = false;
        }

        oldVelocity = _newVelocity;
    }

    public void SetInputDirection(Vector2 _inputDir)
    {
        inputDir = _inputDir;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, collider.size);
        Gizmos.DrawWireCube(transform.position, new Vector2(xLimit * 2, yLimit * 2));
    }

}