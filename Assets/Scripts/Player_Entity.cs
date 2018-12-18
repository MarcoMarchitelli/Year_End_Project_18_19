using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Entity : EntityBase {

    [Header ("Multiple Jump")]
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
    /// Se attivo, il player può fare il primo salto
    /// </summary>
    private bool canJump;

    /// TODO
    /// Eliminare questa variabile quando è finita la fase di testing
    /// <summary>
    /// Quando è true vengono salvati i dati di MultipleJumpsCount quando si è a terra e si può testare il salto multiplo,
    /// Quando è false possono essere settati i valori di MultipleJumpsCount senza che vengano sovrascritti all'istante
    /// </summary>
    [Tooltip("Quando è true viene salvato il valore di MultipleJumpsCount attuale e si può testare il salto multiplo, " +
              "Quando è false può essere settato il valore di MultipleJumpsCount senza che venga sovrascritto all'istante")]
    public bool testing;

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
        base.Start();

        resetMultipleJumpsCount = MultipleJumpsCount;
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

        if (myRayCon.Collisions.below)
        {
            if (!canMultipleJump)
            {
                ResetJump();
            }
            else
            {
                /// TODO
                /// Eliminare questo if quando è finita la fase di testing
                if (!testing)
                {
                    resetMultipleJumpsCount = MultipleJumpsCount;
                }
                ResetJump();
                ResetJumpsCount();
            }
        }

        Vector2 myInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity.x = myInput.x * MovementSpeed;

        //float targetVelocityX = myInput.x * MovementSpeed;

        //velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref VelocityXSmoothing, (myRayCon.Collisions.below) ? AccelerationTimeGrounded : AccelerationTimeAirborne);

        Move(velocity * Time.deltaTime);
    }

    public void Jump()
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

    public void MultipleJump()
    {
        if (canMultipleJump && MultipleJumpsCount > 0)
        {
            velocity.y = ((2 * MultipleJumpHeight) / Mathf.Pow(TimeToJumpApex, 2)) * TimeToJumpApex;
            MultipleJumpsCount--;
        }
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void ResetJumpsCount()
    {
        /// TODO
        /// Eliminare questo if quando è finita la fase di testing
        if (testing)
        {
            MultipleJumpsCount = resetMultipleJumpsCount;
        }
    }

    protected override void Respawn()
    {
        base.Respawn();
        ResetJump();
        ResetJumpsCount();
    }
}