using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityBaseController
{
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

    /// <summary>
    /// Se attivo, il player può fare il primo salto
    /// </summary>
    private bool canJump;

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
        if (Input.GetKeyDown(KeyCode.X))
        {
            Attack();
        }

        #endregion

        ///TODO
        ///Rimuovere finita la fase di debug
        resetMultipleJumpsCount = MultipleJumpsCount;

        //float targetVelocityX = myInput.x * MovementSpeed;

        //velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref VelocityXSmoothing, (myRayCon.Collisions.below) ? AccelerationTimeGrounded : AccelerationTimeAirborne);

        Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (!isDashing)
        {
            /// TODO:
            /// Eliminare quando è finita la fase di testing
            CalculateGravityAndJumpVelocity(ref jumpVelocity, JumpHeight, TimeToJumpApex);
            if (canJump)
            {
                velocity.y = jumpVelocity;
                canJump = false;
            }
        }
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

    public void ResetJump()
    {
        canJump = true;
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
}