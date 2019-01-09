using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastController))]
public abstract class EntityBaseController : MonoBehaviour
{
    /// <summary>
    /// Riferimento alla grafica dell'entità
    /// </summary>
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

    [Header("Statistics")]
    [SerializeField]
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
    /// Velocità con cui l'entità infligge danno
    /// </summary>
    [Tooltip("Velocità con cui l'entità infligge danno")]
    public float AttackSpeed;

    /// <summary>
    /// Tempo che l'entità rimane invulnerabile in secondi
    /// </summary>
    [Tooltip("Tempo che l'entità rimane invulnerabile in secondi")]
    public float InvulnerabiltyTime;

    /// <summary>
    /// Se true, l'entità è considerata viva, altrimenti morta
    /// </summary>
    private bool isAlive = true;

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
    /// Se true, l'entità può sferrare l'attacco
    /// </summary>
    protected bool canHit = true;

    /// <summary>
    /// Gravità subita dall'entità
    /// </summary>
    private float gravity;

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
    /// Curva che indica con che velocità deve accelerare l'entità
    /// </summary>
    [Tooltip("Curva che indica con che velocità deve accelerare l'entità")]
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
    private bool isFacingRight = true;

    /// <summary>
    /// Se true, l'entità sta guardando a sinistra
    /// </summary>
    [Tooltip("Se true, l'entità sta guardando a sinistra")]
    private bool isFacingLeft;

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
    protected bool canDash = true;

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
    protected bool isDashing = false;

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
    /// Gravità da sommare all'entità oltre a quella di base
    /// </summary>
    [Tooltip("Gravità da sommare all'entità oltre a quella di base")]
    public AnimationCurve gravityCurve;

    /// <summary>
    /// Altezza massima che raggiunge il salto
    /// </summary>
    [Tooltip("Altezza massima che raggiunge il salto")]
    public float JumpHeight;

    /// <summary>
    /// Tempo che ci mette il salto a raggiungere il punto più alto
    /// </summary>
    [Tooltip("Tempo che ci mette il salto a raggiungere il punto più alto")]
    public float TimeToJumpApex;

    /// <summary>
    /// Velocità calcolata tramite JumpHeight e TimeToJumpApex
    /// </summary>
    protected float jumpVelocity;

    protected virtual void Start()
    {
        graphic = GetComponentsInChildren<Transform>()[1];

        gravityTimer = new Timer();
        attackTimer = new Timer();
        dashTimer = new Timer();
        invulnerabilityTimer = new Timer();

        SetRespawnVariables();

        CalculateGravityAndJumpVelocity(ref jumpVelocity, JumpHeight, TimeToJumpApex);
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

    public void TakeDamage(int _takenDamage)
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
                Respawn();
            }
            else
            {
                Health -= _takenDamage;
            }

            isInvulnerable = true;
            StartCoroutine(StartInvulnerability());
        }
        else
        {
            Debug.Log("Sei invulnerabile");
        }
    }

    private IEnumerator StartInvulnerability()
    {
        while (!invulnerabilityTimer.CheckTimer(InvulnerabiltyTime) && isInvulnerable)
        {
            invulnerabilityTimer.TickTimer();
            yield return null;
        }

        if (invulnerabilityTimer.CheckTimer(InvulnerabiltyTime) || !isInvulnerable)
        {
            isInvulnerable = false;
            invulnerabilityTimer.StopTimer();
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
        else
        {
            Health += _takenHealth;
        }
    }

    protected void Attack()
    {
        if (canHit)
        {
            Debug.Log("Hai attaccato");
            canHit = false;
            StartCoroutine(Reload());
        }
        else
        {
            Debug.Log("Stai ricaricando l'attacco");
        }
    }

    protected IEnumerator Reload()
    {
        while (!attackTimer.CheckTimer(AttackSpeed) && !canHit)
        {
            attackTimer.TickTimer();
            yield return null;
        }

        if (attackTimer.CheckTimer(AttackSpeed) || canHit)
        {
            canHit = true;
            attackTimer.StopTimer();
        }
    }

    protected void Die()
    {
        isAlive = false;
    }

    protected virtual void Respawn()
    {
        Health = respawnHealth;
        transform.position = respawnPosition;
        isAlive = true;
        canHit = true;
        canDash = true;
        isInvulnerable = false;

    }

    public void SetRespawnVariables()
    {
        SetRespawnHealth();
        SetRespawnPosition();
    }

    public void SetRespawnHealth()
    {
        respawnHealth = Health;
    }

    public void SetRespawnPosition()
    {
        respawnPosition = transform.position;
    }

    public virtual void DeathAnimation()
    {
        /// TODO
        /// Mettere animazione di morte
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

    public void RotateEntity(Vector3 direction, float rotationDegrees)
    {
        graphic.Rotate(direction * rotationDegrees);
    }

    public bool CheckFacingRightAndRotate()   // <------------- ATTENZIONE!!! DEVE ESSERE A TRUE IL PARAMETRO INIZIALE GIUSTO NELL'INSPECTOR
    {
        if (velocity.x > 0 && isFacingLeft)
        {
            isFacingRight = true;
            isFacingLeft = false;
            RotateEntity(Vector3.up, 180f);
            return isFacingRight;
        }
        if (velocity.x < 0 && isFacingRight)
        {
            isFacingLeft = true;
            isFacingRight = false;
            RotateEntity(Vector3.up, 180f);
            return isFacingRight;
        }
        return isFacingRight;
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