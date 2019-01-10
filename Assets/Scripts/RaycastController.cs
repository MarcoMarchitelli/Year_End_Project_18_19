using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class RaycastController : MonoBehaviour
{
    public LayerMask myCollisionMask;

    public float SkinWidth;

    public int HorizontalRayCount;
    public int VerticalRayCount;

    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    [HideInInspector]
    public BoxCollider2D myCollider;
    protected RaycastOrigins myRaycastOrigins;

    /// <summary>
    /// Densità minima dei raycasts
    /// </summary>
    private int raycastsDensity = 5;

    [SerializeField]
    /// <summary>
    /// Lunghezza del raycast superiore
    /// </summary>
    [Tooltip("Lunghezza del raycast superiore")]
    protected float topRayLength;

    [SerializeField]
    /// <summary>
    /// Lunghezza del raycast inferiore
    /// </summary>
    [Tooltip("Lunghezza del raycast inferiore")]
    protected float bottomRayLength;

    [SerializeField]
    /// <summary>
    /// Lunghezza del raycast sinistro
    /// </summary>
    [Tooltip("Lunghezza del raycast sinistro")]
    protected float leftRayLength;

    [SerializeField]
    /// <summary>
    /// Lunghezza del raycast destro
    /// </summary>
    [Tooltip("Lunghezza del raycast destro")]
    protected float rightRayLength;

    public CollisionInfo Collisions;

    protected struct RaycastOrigins
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

    protected virtual void Start()
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

    public virtual void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + SkinWidth;

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? myRaycastOrigins.BottomLeft : myRaycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, directionX * Vector2.right, rayLength, myCollisionMask);

            Debug.DrawRay(rayOrigin, directionX * Vector2.right * rayLength, Color.red);

            if (hit) // Mentre colpisco qualcosa
            {
                velocity.x = (hit.distance - SkinWidth) * directionX;
                rayLength = hit.distance;

                Collisions.left = directionX == -1; // Se la direzione è -1, allora Collisions.left = true
                Collisions.right = directionX == 1; // Se la direzione è 1, allora Collisions.right = true
            }
        }
    }

    public virtual void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + SkinWidth;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? myRaycastOrigins.BottomLeft : myRaycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, directionY * Vector2.up, rayLength, myCollisionMask);

            Debug.DrawRay(rayOrigin, directionY * Vector2.up * rayLength, Color.red);

            if (hit) // Mentre colpisco qualcosa
            {
                velocity.y = (hit.distance - SkinWidth) * directionY;
                rayLength = hit.distance;

                Collisions.below = directionY == -1; // Se la direzione (velocità) è -1, allora Collisions.below = true
                Collisions.above = directionY == 1; // Se la direzione (velocità) è 1, allora Collisions.above = true
            }
        }
    }

    public virtual void CheckRaycastsBools(LayerMask collisionMask)
    {
        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = myRaycastOrigins.BottomLeft;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, leftRayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.left * leftRayLength, Color.red);

            if (hit) // Mentre colpisco qualcosa
            {
                Collisions.left = true;
            }
        }

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = myRaycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, rightRayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * rightRayLength, Color.red);

            if (hit) // Mentre colpisco qualcosa
            {
                Collisions.right = true;
            }
        }

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = myRaycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, topRayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * topRayLength, Color.red);

            if (hit) // Mentre colpisco qualcosa
            {
                Collisions.above = true;
            }
        }

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = myRaycastOrigins.BottomLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, bottomRayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.down * bottomRayLength, Color.red);

            if (hit) // Mentre colpisco qualcosa
            {
                Collisions.below = true;
            }
        }
    }

    protected void CalculateRaySpacing() // Funzione da chiamare ogni volta che si modifica la scala dell'oggetto che usa il raycast
    {
        Bounds myBounds = myCollider.bounds;
        myBounds.Expand(SkinWidth * -2);

        HorizontalRayCount = Mathf.Clamp(HorizontalRayCount, (int)((myBounds.size.y + 1) * raycastsDensity), int.MaxValue);
        VerticalRayCount = Mathf.Clamp(VerticalRayCount, (int)((myBounds.size.x + 1) * raycastsDensity), int.MaxValue);

        horizontalRaySpacing = myBounds.size.y / (HorizontalRayCount - 1);
        verticalRaySpacing = myBounds.size.x / (VerticalRayCount - 1);
    }
}