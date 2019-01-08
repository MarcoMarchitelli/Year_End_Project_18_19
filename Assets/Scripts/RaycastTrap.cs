using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTrap : RaycastController
{
    [SerializeField]
    /// <summary>
    /// Layer delle entità che possono venire danneggiate
    /// </summary>
    [Tooltip("Layer delle entità che possono venire danneggiate")]
    private LayerMask victimMask;

    protected override void Start()
    {
        base.Start();
        UpdateRaycastOrigins();
    }

    private void Update()
    {
        CheckRaycastsBools(victimMask);
    }

    public void DamageVictim(int damageToDo)
    {
        HashSet<Transform> hitVictim = new HashSet<Transform>();

        if (Collisions.above)
        {
            for (int i = 0; i < VerticalRayCount; i++)
            {
                Vector2 rayOrigin = myRaycastOrigins.TopLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, topRayLength, victimMask);

                Debug.DrawRay(rayOrigin, Vector2.up * topRayLength, Color.red);

                if (hit)
                {
                    if (!hitVictim.Contains(hit.transform))
                    {
                        hitVictim.Add(hit.transform);
                        if (hit.collider.GetComponent<PlayerController>() != null)
                        {
                            hit.collider.GetComponent<PlayerController>().TakeDamage(damageToDo); // Viene chiamato ad ogni Update, fixare in caso di lag
                        }
                    }
                }
            }
        }

        if (Collisions.below)
        {
            for (int i = 0; i < VerticalRayCount; i++)
            {
                Vector2 rayOrigin = myRaycastOrigins.BottomLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, bottomRayLength, victimMask);

                Debug.DrawRay(rayOrigin, Vector2.down * bottomRayLength, Color.red);

                if (hit)
                {
                    if (!hitVictim.Contains(hit.transform))
                    {
                        hitVictim.Add(hit.transform);
                        if (hit.collider.GetComponent<PlayerController>() != null)
                        {
                            hit.collider.GetComponent<PlayerController>().TakeDamage(damageToDo); // Viene chiamato ad ogni Update, fixare in caso di lag
                        }
                    }
                }
            }
        }

        if (Collisions.right)
        {
            for (int i = 0; i < HorizontalRayCount; i++)
            {
                Vector2 rayOrigin = myRaycastOrigins.BottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, rightRayLength, victimMask);

                Debug.DrawRay(rayOrigin, Vector2.right * rightRayLength, Color.red);

                if (hit)
                {
                    if (!hitVictim.Contains(hit.transform))
                    {
                        hitVictim.Add(hit.transform);
                        if (hit.collider.GetComponent<PlayerController>() != null)
                        {
                            hit.collider.GetComponent<PlayerController>().TakeDamage(damageToDo); // Viene chiamato ad ogni Update, fixare in caso di lag
                        }
                    }
                }
            }
        }

        if (Collisions.left)
        {
            for (int i = 0; i < HorizontalRayCount; i++)
            {
                Vector2 rayOrigin = myRaycastOrigins.BottomLeft;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, leftRayLength, victimMask);

                Debug.DrawRay(rayOrigin, Vector2.left * leftRayLength, Color.red);

                if (hit)
                {
                    if (!hitVictim.Contains(hit.transform))
                    {
                        hitVictim.Add(hit.transform);
                        if (hit.collider.GetComponent<PlayerController>() != null)
                        {
                            hit.collider.GetComponent<PlayerController>().TakeDamage(damageToDo); // Viene chiamato ad ogni Update, fixare in caso di lag
                        }
                    }
                }
            }
        }
    }
}
