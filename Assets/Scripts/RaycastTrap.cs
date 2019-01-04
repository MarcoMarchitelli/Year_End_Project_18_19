using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTrap : RaycastController
{
    [SerializeField]
    /// <summary>
    /// Lunghezza del raycast superiore
    /// </summary>
    [Tooltip("Lunghezza del raycast superiore")]
    private float topRayLength;

    [SerializeField]
    /// <summary>
    /// Lunghezza del raycast inferiore
    /// </summary>
    [Tooltip("Lunghezza del raycast inferiore")]
    private float bottomRayLength;

    [SerializeField]
    /// <summary>
    /// Lunghezza del raycast sinistro
    /// </summary>
    [Tooltip("Lunghezza del raycast sinistro")]
    private float leftRayLength;

    [SerializeField]
    /// <summary>
    /// Lunghezza del raycast destro
    /// </summary>
    [Tooltip("Lunghezza del raycast destro")]
    private float rightRayLength;

    [SerializeField]
    /// <summary>
    /// Layer delle entità che possono venire danneggiate
    /// </summary>
    [Tooltip("Layer delle entità che possono venire danneggiate")]
    private LayerMask victimMask;

    /// <summary>
    /// Entità colpita dalla trappola
    /// </summary>
    private RaycastHit2D hit;

    protected override void Start()
    {
        base.Start();
        UpdateRaycastOrigins();
    }

    private void Update()
    {
        hit = ShowRaycasts(leftRayLength, rightRayLength, topRayLength, bottomRayLength, victimMask);
    }

    public void DamageVictim(int damageToDo)
    {
        HashSet<Transform> hitVictim = new HashSet<Transform>();

        if (Collisions.above)
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

        if (Collisions.below)
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

        if (Collisions.right)
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

        if (Collisions.left)
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
