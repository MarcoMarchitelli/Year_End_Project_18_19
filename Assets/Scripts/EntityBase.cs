using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastPlayer))]
public abstract class EntityBase : MonoBehaviour
{
    /// <summary>
    /// Riferimento allo script RaycastController dell'entità
    /// </summary>
    public RaycastPlayer myRayCon;

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
    /// Se true, l'entità è considerata viva, altrimenti morta
    /// </summary>
    private bool isAlive = true;

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
    public Vector3 velocity;

    [Header("Movement")]
    /// <summary>
    /// Velocità di movimento dell'entità
    /// </summary>
    [Tooltip("Velocità di movimento dell'entità")]
    public float MovementSpeed;

    /// <summary>
    /// Tempo che ci mette a raggiungere il punto desiderato mentre si è a terra
    /// </summary>
    //public float AccelerationTimeGrounded;

    /// <summary>
    /// Tempo che ci mette a raggiungere il punto desiderato mentre si è in aria
    /// </summary>
    //public float AccelerationTimeAirborne;

    /// <summary>
    /// Variabile utile alla funzione di Smoothing del movimento
    /// </summary>
    protected float VelocityXSmoothing;

    [Header ("Jump")]
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
        myRayCon = GetComponent<RaycastPlayer>();

        SetRespawnVariables();

        CalculateGravityAndJumpVelocity(ref jumpVelocity, JumpHeight, TimeToJumpApex);
    }

    protected virtual void Update()
    {
        velocity.y += Gravity * Time.deltaTime;
    }

    protected virtual void Move(Vector3 movingVelocity)
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
    }

    protected void CalculateGravityAndJumpVelocity(ref float currentJumpVelocity, float currentJumpHeight, float currentTimeToJumpApex)
    {
        Gravity = -(2 * currentJumpHeight) / Mathf.Pow(currentTimeToJumpApex, 2);
        currentJumpVelocity = Mathf.Abs(Gravity) * currentTimeToJumpApex;
    }

    public void TakeDamage(int _takenDamage)
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

    protected virtual void Attack()
    {
        if (canHit)
        {
            TakeDamage(AttackDamage);
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
        float timer = 0;
        while (timer <= AttackSpeed)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (timer >= AttackSpeed)
        {
            canHit = true;
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
    }

    public void SetRespawnVariables()
    {
        respawnHealth = Health;
        respawnPosition = transform.position;
    }

    public virtual void DeathAnimation()
    {
        /// TODO
        /// Mettere animazione di morte
    }
}