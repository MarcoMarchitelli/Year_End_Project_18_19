using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastPlayer : MonoBehaviour
{
    public LayerMask myCollisionMask;
    public RaycastHit2D Hit;

    public float SkinWidth;

    public int HorizontalRayCount;
    public int VerticalRayCount;

    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    BoxCollider2D myCollider;
    RaycastOrigins myRaycastOrigins;

    public CollisionInfo Collisions;

    struct RaycastOrigins
    {
        public Vector2 TopLeft, TopRight;
        public Vector2 BottomLeft, BottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;


        public void ResetCollisionInfo()
        {
            above = below = false;
            left = right = false;
        }
    }

    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        Bounds myBounds = myCollider.bounds;
        myBounds.Expand(SkinWidth * -2);

        myRaycastOrigins.TopLeft = new Vector2(myBounds.min.x, myBounds.max.y);
        myRaycastOrigins.TopRight = new Vector2(myBounds.max.x, myBounds.max.y);
        myRaycastOrigins.BottomLeft = new Vector2(myBounds.min.x, myBounds.min.y);
        myRaycastOrigins.BottomRight = new Vector2(myBounds.max.x, myBounds.min.y);
    }

    public void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + SkinWidth;

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? myRaycastOrigins.BottomLeft : myRaycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            Hit = Physics2D.Raycast(rayOrigin, directionX * Vector2.right, rayLength, myCollisionMask);

            Debug.DrawRay(rayOrigin, directionX * Vector2.right * rayLength, Color.red);

            if (Hit) // Mentre colpisco qualcosa
            {
                if (Hit.collider.gameObject.layer != LayerMask.NameToLayer("Platform"))
                {
                    velocity.x = (Hit.distance - SkinWidth) * directionX;
                    rayLength = Hit.distance;

                    Collisions.left = directionX == -1; // Se la direzione è -1, allora Collisions.left = true
                    Collisions.right = directionX == 1; // Se la direzione è 1, allora Collisions.right = true
                }
            }
        }
    }

    public void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + SkinWidth;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? myRaycastOrigins.BottomLeft : myRaycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            Hit = Physics2D.Raycast(rayOrigin, directionY * Vector2.up, rayLength, myCollisionMask);

            Debug.DrawRay(rayOrigin, directionY * Vector2.up * rayLength, Color.red);

            if (Hit) // Mentre colpisco qualcosa
            {
                if (Hit.collider.gameObject.layer == LayerMask.NameToLayer("Platform") && directionY == 1)
                {
                    Collisions.above = false;
                }
                else if (Hit.collider.gameObject.layer == LayerMask.NameToLayer("Platform") && Input.GetAxisRaw("Vertical") == -1)
                {
                    Collisions.below = false;
                }
                else
                {
                    velocity.y = (Hit.distance - SkinWidth) * directionY;
                    rayLength = Hit.distance;

                    Collisions.below = directionY == -1; // Se la direzione (velocità) è -1, allora Collisions.below = true
                    Collisions.above = directionY == 1; // Se la direzione (velocità) è 1, allora Collisions.above = true
                }
            }
        }
    }

    void CalculateRaySpacing()
    {
        Bounds myBounds = myCollider.bounds;
        myBounds.Expand(SkinWidth * -2);

        HorizontalRayCount = Mathf.Clamp(HorizontalRayCount, 2, int.MaxValue);
        VerticalRayCount = Mathf.Clamp(VerticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = myBounds.size.y / (HorizontalRayCount - 1);
        verticalRaySpacing = myBounds.size.x / (VerticalRayCount - 1);
    }
}