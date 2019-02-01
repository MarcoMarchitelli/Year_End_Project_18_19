using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastPlayer))]
public class PlayerController : EntityBaseController
{
    /// <summary>
    /// Riferimento al CanvasManager
    /// </summary>
    private CanvasManager cm;

    [Header("Multiple Jump")]
    /// <summary>
    /// Se attivato, il player può fare più salti consecutivi
    /// </summary>
    [Tooltip("Se attivato, il player può fare più salti consecutivi")]
    public bool canMultipleJump;

    /// <summary>
    /// Numero di salti consecutivi che può fare il player
    /// </summary>
    [Tooltip("Numero di salti consecutivi che può fare il player")]
    public int MultipleJumpsCount;

    /// <summary>
    /// Altezza massima che raggiunge il salto dal secondo in poi
    /// </summary>
    [Tooltip("Altezza massima che raggiunge il salto dal secondo in poi")]
    public float MultipleJumpHeight;

    /// <summary>
    /// Serve a salvare il numero di salti consecutivi che può fare il player
    /// </summary>
    private int resetMultipleJumpsCount;

    /// <summary>
    /// Salto consecutivo a cui è il player
    /// </summary>
    private int currentMultipleJumpsCount;

    [Header("Debug Values")]
    /// <summary>
    /// Danno da ricevere (Debug)
    /// </summary>
    [Tooltip("Danno da ricevere (Debug)")]
    public int DamageToTake;

    /// <summary>
    /// Cura da ricevere (Debug)
    /// </summary>
    [Tooltip("Cura da ricevere (Debug)")]
    public int HealthToReceive;

    protected override void Start()
    {
        myRayCon = GetComponent<RaycastPlayer>();
        cm = FindObjectOfType<CanvasManager>();

        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        #region DebugInput

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(DamageToTake);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Heal(HealthToReceive);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Die();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Respawn();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetRespawnPosition();
        }

        #endregion

        ///TODO
        ///Rimuovere finita la fase di debug
        resetMultipleJumpsCount = MultipleJumpsCount;

        Move(velocity * Time.deltaTime);
    }

    public void MultipleJump()
    {
        if (!isDashing)
        {
            if (canMultipleJump && currentMultipleJumpsCount > 0)
            {
                velocity.y = ((2 * MultipleJumpHeight) / Mathf.Pow(TimeToJumpApex, 2)) * TimeToJumpApex;
                currentMultipleJumpsCount--;
            }
        }
    }

    public void ResetJumpsCount()
    {
        currentMultipleJumpsCount = resetMultipleJumpsCount;
    }

    protected override void Respawn()
    {
        base.Respawn();
        ResetJump();
        ResetJumpsCount();
    }

    public override void TakeDamage(int _takenDamage)
    {
        base.TakeDamage(_takenDamage);
        cm.UpdateLifeBar(Health);
    }
}