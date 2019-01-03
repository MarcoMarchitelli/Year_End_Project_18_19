using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastTrap))]
public class TrapController : MonoBehaviour
{
    /// <summary>
    /// Riferimento allo script RaycastController della trappola
    /// </summary>
    public RaycastTrap myRayCon;

    [SerializeField]
    /// <summary>
    /// Danno inflitto dalla trappola
    /// </summary>
    [Tooltip("Danno inflitto dalla trappola")]
    private int trapDamage;

    private void Start()
    {
        myRayCon = GetComponent<RaycastTrap>();
    }

    void Update()
    {
        myRayCon.DamageVictim(trapDamage);
    }
}
