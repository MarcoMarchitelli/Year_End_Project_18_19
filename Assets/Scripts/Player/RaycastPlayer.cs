using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPlayer : RaycastController
{
    public override void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + SkinWidth;

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? myRaycastOrigins.BottomLeft : myRaycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, directionX * Vector2.right, rayLength, GeneralMask);

            Debug.DrawRay(rayOrigin, directionX * Vector2.right * rayLength, Color.red);

            if (hit) // Mentre colpisco qualcosa
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Trap"))
                {
                    GetComponent<PlayerController>().TakeDamage(hit.collider.GetComponent<TrapController>().trapDamage);
                }
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Platform") && hit.distance == 0)
                {
                    continue;
                }
                velocity.x = (hit.distance - SkinWidth) * directionX;
                rayLength = hit.distance;

                Collisions.left = directionX == -1; // Se la direzione è -1, allora Collisions.left = true
                Collisions.right = directionX == 1; // Se la direzione è 1, allora Collisions.right = true
            }
        }
    }

    public override void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + SkinWidth;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? myRaycastOrigins.BottomLeft : myRaycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, directionY * Vector2.up, rayLength, GeneralMask);

            Debug.DrawRay(rayOrigin, directionY * Vector2.up * rayLength, Color.red);

            if (hit) // Mentre colpisco qualcosa
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Trap"))
                {
                    GetComponent<PlayerController>().TakeDamage(hit.collider.GetComponent<TrapController>().trapDamage);
                }
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Platform") && directionY == 1)
                {
                    if (hit.distance == 0)
                    {
                        continue;
                    }
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Platform") && (Mathf.Sign(Input.GetAxisRaw("Vertical")) == -1))
                {
                    Collisions.below = false;
                }
                else
                {
                    velocity.y = (hit.distance - SkinWidth) * directionY;
                    rayLength = hit.distance;

                    Collisions.below = directionY == -1; // Se la direzione (velocità) è -1, allora Collisions.below = true
                    Collisions.above = directionY == 1; // Se la direzione (velocità) è 1, allora Collisions.above = true
                }
            }
        }
    }
}
