using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTrap : RaycastController
{ 
    [SerializeField]
    /// <summary>
    /// Lunghezza del raycast verticale
    /// </summary>
    [Tooltip("Lunghezza del raycast verticale")]
    private float verticalRayLength;

    [SerializeField]
    /// <summary>
    /// Lunghezza del raycast orizzontale
    /// </summary>
    [Tooltip("Lunghezza del raycast orizzontale")]
    private float horizontalRayLength;

    [SerializeField]
    /// <summary>
    /// Layer delle entità che possono venire danneggiate
    /// </summary>
    [Tooltip("Layer delle entità che possono venire danneggiate")]
    private LayerMask victimMask;

    private void Update()
    {
        UpdateRaycastOrigins();
    }

    public void DamageVictim(int damageToDo)
    {
        HashSet<Transform> hitVictim = new HashSet<Transform>();

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = myRaycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, verticalRayLength, victimMask);

            Debug.DrawRay(rayOrigin, Vector2.up * verticalRayLength, Color.red);

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
