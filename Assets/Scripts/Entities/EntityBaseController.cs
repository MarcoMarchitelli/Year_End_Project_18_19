﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EntityBaseController : MonoBehaviour, IDamageable
{
    [SerializeField]
    /// <summary>
    /// Riferimento alla grafica dell'entità
    /// </summary>
    [Tooltip("Riferimento alla grafica dell'entità")]
    private Transform graphic;

    [HideInInspector]
    /// <summary>
    /// Riferimento allo script RaycastController dell'entità
    /// </summary>
    public RaycastController myRayCon;

    /// <summary>
    /// Tempo che scandisce la curva di gravità
    /// </summary>
    private Timer gravityTimer;

    /// <summary>
    /// variabile utile per utilizzare funzioni di tipo Timer per l'attacco
    /// </summary>
    private Timer attackTimer;

    /// <summary>
    /// variabile utile per utilizzare funzioni di tipo Timer per il dash
    /// </summary>
    private Timer dashTimer;

    /// <summary>
    /// variabile utile per utilizzare funzioni di tipo Timer per l'invulnerabilità
    /// </summary>
    private Timer invulnerabilityTimer;

    /// <summary>
    /// variabile utile per utilizzare funzioni di tipo Timer per la morte
    /// </summary>
    private Timer deathTimer;

    /// <summary>
    /// Vita attuale dell'entità
    /// </summary>
    [Tooltip("Vita attuale dell'entità")]
    private int health;

    /// <summary>
    /// Proprietà della variabile "health"
    /// </summary>
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    [Header("Statistics")]
    /// <summary>
    /// Vita massima dell'entità
    /// </summary>
    [Tooltip("Vita massima dell'entità")]
    public int MaxHealth;

    /// <summary>
    /// Danno che infligge l'entità
    /// </summary>
    [Tooltip("Danno che infligge l'entità")]
    public int AttackDamage;

    /// <summary>
    /// Tempo che ci mette l'attacco ad essere concluso (in secondi)
    /// </summary>
    [Tooltip("Tempo che ci mette l'attacco ad essere concluso (in secondi)")]
    public float AttackTime;

    /// <summary>
    /// Velocità con cui l'entità infligge danno
    /// </summary>
    [Tooltip("Velocità con cui l'entità infligge danno")]
    public float AttackSpeed;

    /// <summary>
    /// Se true, l'entità può sferrare l'attacco
    /// </summary>
    protected bool canAttack = true;

    /// <summary>
    /// Se true, l'entità sta ricaricando l'attacco
    /// </summary>
    protected bool isAttackRecharging;

    /// <summary>
    /// Se true, l'entità sta eseguendo un attacco
    /// </summary>
    protected bool isAttacking;

    /// <summary>
    /// Tempo che l'entità rimane invulnerabile in secondi
    /// </summary>
    [Tooltip("Tempo che l'entità rimane invulnerabile in secondi")]
    public float InvulnerabiltyTime;

    /// <summary>
    /// Tempo che l'entità ci mette a respawnare in secondi
    /// </summary>
    [Tooltip("Tempo che l'entità ci mette a respawnare in secondi")]
    public float RespawnTime;

    /// <summary>
    /// Se true, l'entità è considerata viva, altrimenti morta
    /// </summary>
    protected bool isAlive = true;

    /// <summary>
    /// Se true, l'entità è invulnerabile
    /// </summary>
    private bool isInvulnerable = false;

    /// <summary>
    /// Serve a salvare la vita iniziale dell'entità
    /// </summary>
    private int respawnHealth;

    /// <summary>
    /// Serve a salvare la posizione iniziale dell'entità
    /// </summary>
    private Vector3 respawnPosition;

    /// <summary>
    /// Gravità subita dall'entità
    /// </summary>
    protected float gravity;

    /// <summary>
    /// Proprietà della variabile "gravity"
    /// </summary>
    public float Gravity
    {
        get { return gravity; }
        private set { gravity = value; }
    }

    /// <summary>
    /// Vector3 che misura la velocità dell'entità in ogni direzione
    /// </summary>
    protected Vector3 velocity;

    [Header("Movement")]
    /// <summary>
    /// Se true può correre
    /// </summary>
    [Tooltip("Se true può correre")]
    public bool CanRun;

    /// <summary>
    /// Velocità di movimento dell'entità
    /// </summary>
    [Tooltip("Velocità di movimento dell'entità")]
    public float MovementSpeed;

    /// <summary>
    /// Valore massimo che può raggiungere la corsa (Oltre alla velocità di movimento)
    /// </summary>
    [Tooltip("Valore massimo che può raggiungere la corsa (Oltre alla velocità di movimento)")]
    public float MaxRunValue;

    /// <summary>
    /// Curva di corsa - X: Secondi dall'inizio della corsa, Y: Velocità di corsa sommata alla velocità base di movimento
    /// </summary>
    [Tooltip("Curva di corsa - X: Secondi dall'inizio della corsa, Y: Velocità di corsa sommata alla velocità base di movimento")]
    public AnimationCurve RunningCurve;

    /// <summary>
    /// Tempo raggiunto ogni secondo
    /// </summary>
    private float AccelerationTime;

    /// <summary>
    /// Valore attuale della corsa
    /// </summary>
    protected float runningValue;

    /// <summary>
    /// Se true, l'entità sta guardando a destra
    /// </summary>
    [Tooltip("Se true, l'entità sta guardando a destra")]
    private bool isFacingRight;

    /// <summary>
    /// Se true, l'entità sta guardando a sinistra
    /// </summary>
    [Tooltip("Se true, l'entità sta guardando a sinistra")]
    private bool isFacingLeft;

    /// <summary>
    /// Se true, l'entità sta guardando sopra
    /// </summary>
    [Tooltip("Se true, l'entità sta guardando sopra")]
    private bool isFacingUp;

    /// <summary>
    /// Se true, l'entità sta guardando sotto
    /// </summary>
    [Tooltip("Se true, l'entità sta guardando sotto")]
    private bool isFacingDown;

    [Header("Dash")]
    /// <summary>
    /// Se true, il player ha sbloccato il dash
    /// </summary>
    [Tooltip("Se true, il player può fare il dash")]
    public bool isDashUnlocked;

    /// <summary>
    /// Se true, il player può fare il dash
    /// </summary>
    [Tooltip("Se true, il player può fare il dash")]
    public bool canDash;

    /// <summary>
    /// Distanza percorsa dal dash
    /// </summary>
    [Tooltip("Distanza percorsa dal dash")]
    public float DashDistance;

    /// <summary>
    /// Tempo che ci mette il dash ad essere concluso (in secondi)
    /// </summary>
    [Tooltip("Tempo che ci mette il dash ad essere concluso (in secondi)")]
    public float DashTime;

    /// <summary>
    /// Tempo di ricarica del dash
    /// </summary>
    [Tooltip("Tempo di ricarica del dash")]
    public float DashRechargeTime;

    /// <summary>
    /// Punto in cui il dash ha raggiunto la massima distanza
    /// </summary>
    private float dashArrivePoint;

    /// <summary>
    /// Valore attuale del dash
    /// </summary>
    protected float dashingValue;

    /// <summary>
    /// Se true, l'entità sta eseguendo un dash
    /// </summary>
    protected bool isDashing;

    /// <summary>
    /// Se true, l'entità sta ricaricando il dash
    /// </summary>
    protected bool isDashRecharging;

    /// <summary>
    /// Variabile utile alla funzione di Smoothing del movimento
    /// </summary>
    protected float VelocityXSmoothing;

    [Header("Jump")]
    /// <summary>
    /// Curva di gravità - X: Secondi dall'inizio della caduta, Y: Gravità da sommare alla normale gravità di caduta
    /// </summary>
    [Tooltip("Curva di gravità - X: Secondi dall'inizio della caduta, Y: Gravità da sommare alla normale gravità di caduta")]
    public AnimationCurve gravityCurve;

    /// <summary>
    /// Altezza massima che raggiunge il salto
    /// </summary>
    [Tooltip("Altezza massima che raggiunge il salto")]
    public float MaxJumpHeight;

    /// <summary>
    /// Altezza minima che raggiunge il salto
    /// </summary>
    [Tooltip("Altezza minima che raggiunge il salto")]
    public float MinJumpHeight;

    /// <summary>
    /// Tempo che ci mette il salto a raggiungere il punto più alto
    /// </summary>
    [Tooltip("Tempo che ci mette il salto a raggiungere il punto più alto")]
    public float TimeToJumpApex;

    /// <summary>
    /// Velocità calcolata tramite JumpHeight e TimeToJumpApex
    /// </summary>
    protected float maxJumpVelocity;

    /// <summary>
    /// Minimum jump velocity.
    /// </summary>
    protected float minJumpVelocity;

    /// <summary>
    /// Se attivo, l'entità può fare il primo salto
    /// </summary>
    private bool canJump;

    protected virtual void Start()
    {
        gravityTimer = new Timer();
        attackTimer = new Timer();
        dashTimer = new Timer();
        invulnerabilityTimer = new Timer();
        deathTimer = new Timer();

        SetRespawnVariables();
        Health = respawnHealth;

        CalculateGravityAndJumpVelocity(ref maxJumpVelocity, MaxJumpHeight, TimeToJumpApex);
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity)) * MinJumpHeight;
    }

    protected virtual void Update()
    {
        SufferGravity();
    }

    public void SufferGravity()
    {
        if (!isDashing)
        {
            velocity.y += Gravity * Time.deltaTime;
        }
    }

    public void AddGravity()
    {
        gravityTimer.TickTimer();
        velocity.y += gravityCurve.Evaluate(gravityTimer.GetTimer() * Time.deltaTime);
    }

    public void ResetGravityTimer()
    {
        gravityTimer.StopTimer();
    }

    public void Move(Vector3 movingVelocity, bool isStandingOnMovingPlatform = false)
    {
        myRayCon.UpdateRaycastOrigins();

        myRayCon.Collisions.ResetCollisionInfo();

        if (velocity.x != 0)
        {
            myRayCon.HorizontalCollisions(ref movingVelocity);
        }

        if (velocity.y != 0)
        {
            myRayCon.VerticalCollisions(ref movingVelocity);
        }

        transform.Translate(movingVelocity);

        if (isStandingOnMovingPlatform)
        {
            myRayCon.Collisions.below = true;
        }
    }

    protected void CalculateGravityAndJumpVelocity(ref float currentJumpVelocity, float currentJumpHeight, float currentTimeToJumpApex)
    {
        Gravity = -(2 * currentJumpHeight) / Mathf.Pow(currentTimeToJumpApex, 2);
        currentJumpVelocity = Mathf.Abs(Gravity) * currentTimeToJumpApex;
    }

    public virtual void TakeDamage(int _takenDamage)
    {
        if (!isInvulnerable)
        {
            if (_takenDamage < 0)
            {
                Heal(_takenDamage);
            }
            if (Health - _takenDamage <= 0)
            {
                Health = 0;
            }
            if (Health <= 0)
            {
                Die();
            }
            else
            {
                Health -= _takenDamage;
                isInvulnerable = true;
                StartCoroutine(StartInvulnerability());
            }
        }
    }

    private IEnumerator StartInvulnerability()
    {
        while (!invulnerabilityTimer.CheckTimer(InvulnerabiltyTime) && isInvulnerable)
        {
            invulnerabilityTimer.TickTimer();
            yield return null;
        }

        if (invulnerabilityTimer.CheckTimer(InvulnerabiltyTime) || !isInvulnerable || InvulnerabiltyTime <= 0f)
        {
            isInvulnerable = false;
            invulnerabilityTimer.StopTimer();
        }
    }

    protected IEnumerator StartRespawn()
    {
        while (!deathTimer.CheckTimer(RespawnTime))
        {
            deathTimer.TickTimer();
            yield return null;
        }

        if (deathTimer.CheckTimer(RespawnTime))
        {
            Respawn();
            deathTimer.StopTimer();
        }
    }

    public void Heal(int _takenHealth)
    {
        if (_takenHealth < 0)
        {
            TakeDamage(_takenHealth);
        }
        if (Health + _takenHealth >= MaxHealth)
        {
            Health = MaxHealth;
        }
        if (Health <= 0)
        {
            Die();
        }
        else
        {
            Health += _takenHealth;
        }
    }

    public void BasicAttack()
    {
        if (canAttack && isAlive && !CanvasManager.isPaused)
        {
            if (isFacingRight && !(isFacingUp || isFacingDown))
            {
                List<IDamageable> hitDamageableList = myRayCon.TriggerAttackRaycasts(true);
                canAttack = false;
                isAttacking = true;
                foreach (IDamageable damageable in hitDamageableList)
                {
                    damageable.TakeDamage(AttackDamage);
                }
            }

            if (isFacingLeft && !(isFacingUp || isFacingDown))
            {
                List<IDamageable> hitDamageableList = myRayCon.TriggerAttackRaycasts(false, true);
                canAttack = false;
                isAttacking = true;
                foreach (IDamageable damageable in hitDamageableList)
                {
                    damageable.TakeDamage(AttackDamage);
                }
            }

            if (isFacingUp)
            {
                List<IDamageable> hitDamageableList = myRayCon.TriggerAttackRaycasts(false, false, true);
                canAttack = false;
                isAttacking = true;
                foreach (IDamageable damageable in hitDamageableList)
                {
                    damageable.TakeDamage(AttackDamage);
                }
            }

            if (isFacingDown && !myRayCon.Collisions.below)
            {
                List<IDamageable> hitDamageableList = myRayCon.TriggerAttackRaycasts(false, false, false, true);
                canAttack = false;
                isAttacking = true;
                foreach (IDamageable damageable in hitDamageableList)
                {
                    damageable.TakeDamage(AttackDamage);
                }
            }
        }
    }

    public void SetIsAttacking()
    {
        if (!attackTimer.CheckTimer(AttackTime))
        {
            attackTimer.TickTimer();
        }
        else
        {
            attackTimer.StopTimer();
            isAttacking = false;
            isAttackRecharging = true;
        }
    }

    public void RechargeAttack()
    {
        if (!attackTimer.CheckTimer(AttackSpeed))
        {
            attackTimer.TickTimer();
        }
        else
        {
            attackTimer.StopTimer();
            canAttack = true;
            isAttackRecharging = false;
        }
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }

    public bool GetIsAttackRecharging()
    {
        return isAttackRecharging;
    }

    public void JumpMax()
    {
        if (!isDashing && isAlive && !CanvasManager.isPaused)
        {
            if (canJump)
            {
                velocity.y = maxJumpVelocity;
                canJump = false;
            }
        }
    }

    public void JumpMin()
    {
        if (!isDashing && isAlive && !CanvasManager.isPaused)
        {
            velocity.y = minJumpVelocity;
            canJump = false;
        }
    }

    public void ResetJump()
    {
        canJump = true;
    }

    protected void Die()
    {
        isAlive = false;
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }

    public virtual void Respawn()
    {
        isAlive = true;
        Health = respawnHealth;
        transform.position = respawnPosition;
        canAttack = true;
        canDash = true;
        isInvulnerable = false;
        ResetHorizontalVelocity();
        ResetVerticalVelocity();
        isDashing = false;
        isAttacking = false;
    }

    public void SetRespawnVariables()
    {
        SetRespawnHealth();
        SetRespawnPosition();
    }

    public void SetRespawnHealth()
    {
        respawnHealth = MaxHealth;
    }

    public void SetRespawnPosition()
    {
        respawnPosition = transform.position;
    }

    public void ResetHorizontalVelocity()
    {
        velocity.x = 0;
    }

    public void ResetVerticalVelocity()
    {
        velocity.y = 0;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void SetHorizontalVelocity(float rawAxis)
    {
        if (!isDashing)
        {
            if (CanRun)
            {
                SetRunningValue(rawAxis);
            }
            velocity.x = MovementSpeed * rawAxis + runningValue;
        }
    }

    public void Dash()
    {
        if (isAlive && !CanvasManager.isPaused)
        {
            if (isFacingRight)
            {
                dashArrivePoint = transform.localPosition.x + DashDistance;
            }
            else
            {
                dashArrivePoint = transform.localPosition.x - DashDistance;
            }

            if (isDashUnlocked)
            {
                if (canDash)
                {
                    ResetVerticalVelocity();
                    SetDashingValue();
                    velocity.x = dashingValue;
                    isDashing = true;
                    canDash = false;
                }
                else
                {
                    Debug.Log("Stai ricaricando il dash");
                }
            }
        }
    }

    public void RechargeDash()
    {
        if (!dashTimer.CheckTimer(DashRechargeTime))
        {
            dashTimer.TickTimer();
        }
        else
        {
            dashTimer.PauseTimer();
            if (myRayCon.Collisions.below)
            {
                dashTimer.StopTimer();
                canDash = true;
                isDashRecharging = false;
            }
        }
    }

    private void SetDashingValue()
    {
        if (isFacingRight)
        {
            dashingValue = DashDistance / DashTime;
        }
        else
        {
            dashingValue = -DashDistance / DashTime;
        }
    }

    private void SetRunningValue(float direction)
    {
        runningValue = RunningCurve.Evaluate(AccelerationTime) * MaxRunValue * direction;
    }

    public void UpdateAccelerationTime()
    {
        AccelerationTime += Time.deltaTime;
    }

    public void ResetAccelerationTime()
    {
        AccelerationTime = 0f;
    }

    public void RotateEntity()
    {
        if (isAlive && !CanvasManager.isPaused)
        {
            if (isFacingLeft)
            {
                graphic.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (isFacingRight)
            {
                graphic.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }

    public void ResetFacingDirections()
    {
        SetIsFacingLeft(false);
        SetIsFacingRight(false);
        SetIsFacingUp(false);
        SetIsFacingDown(false);
    }

    public void SetIsFacingLeft(bool value)
    {
        if (isAlive && !isDashing && !CanvasManager.isPaused)
        {
            isFacingLeft = value;
        }
    }

    public bool GetIsFacingLeft()
    {
        return isFacingLeft;
    }

    public void SetIsFacingRight(bool value)
    {
        if (isAlive && !isDashing && !CanvasManager.isPaused)
        {
            isFacingRight = value;
        }
    }

    public bool GetIsFacingRight()
    {
        return isFacingRight;
    }

    public void SetIsFacingUp(bool value)
    {
        isFacingUp = value;
    }

    public bool GetIsFacingUp()
    {
        return isFacingUp;
    }

    public void SetIsFacingDown(bool value)
    {
        isFacingDown = value;
    }

    public bool GetIsFacingDown()
    {
        return isFacingDown;
    }

    public void SetIsDashing()
    {
        if (isFacingRight)
        {
            if (transform.localPosition.x >= dashArrivePoint || myRayCon.Collisions.right)
            {
                isDashing = false;
                isDashRecharging = true;
            }
            else
            {
                isDashing = true;
            }
        }
        else
        {
            if (transform.localPosition.x <= dashArrivePoint || myRayCon.Collisions.left)
            {
                isDashing = false;
                isDashRecharging = true;
            }
            else
            {
                isDashing = true;
            }
        }
    }

    public bool GetIsDashing()
    {
        return isDashing;
    }

    public bool GetCanDash()
    {
        return canDash;
    }

    public bool GetIsDashRecharging()
    {
        return isDashRecharging;
    }
}