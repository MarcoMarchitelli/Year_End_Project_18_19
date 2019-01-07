using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastPlatform))]
public class PlatformController : MonoBehaviour
{
    [HideInInspector]
    /// <summary>
    /// Riferimento allo script RaycastController della piattaforma
    /// </summary>
    public RaycastPlatform myRayCon;

    /// <summary>
    /// <summary>
    /// variabile utile per utilizzare funzioni di tipo Timer per lo shake della piattaforma
    /// </summary>
    private Timer shakeTimer;

    /// <summary>
    /// variabile utile per utilizzare funzioni di tipo Timer per il fade della piattaforma
    /// </summary>
    private Timer fadeTimer;

    /// <summary>
    /// variabile utile per utilizzare funzioni di tipo Timer per il ritorno della piattaforma
    /// </summary>
    private Timer returnTimer;

    /// <summary>
    /// variabile utile per utilizzare funzioni di tipo Timer per il tremolio della piattaforma
    /// </summary>
    private Timer trembleTimer;

    /// <summary>
    /// variabile utile per utilizzare funzioni di tipo Timer per la caduta della piattaforma
    /// </summary>
    private Timer fallTimer;

    [Header("Moving Platform")]
    /// Velocità con cui si muove la piattaforma
    /// </summary>
    [Tooltip("Velocità con cui si muove la piattaforma")]
    public float MovementSpeed;

    /// <summary>
    /// Valore di prova per simulare vari platform
    /// </summary>
    [Tooltip("Valore di prova per simulare vari platform")]
    [Range(0, 2)]
    public float easeAmount;

    /// <summary>
    /// Tempo che deve aspettare la piattaforma prima di andare al punto successivo
    /// </summary>
    [Tooltip("Tempo che deve aspettare la piattaforma prima di andare al punto successivo")]
    public float WaitTime;

    /// <summary>
    /// Se true, quando a destinazione torna al primo punto e ricomincia il giro
    /// </summary>
    [Tooltip("Se true, quando a destinazione torna al primo punto e ricomincia il giro")]
    public bool Cyclic;

    /// <summary>
    /// Se true, quando arriva a destinazione rifà il giro al contrario
    /// </summary>
    [Tooltip("Se true, quando arriva a destinazione rifà il giro al contrario")]
    public bool Backward;

    /// <summary>
    /// Punti da cui passerà la piattaforma
    /// </summary>
    [Tooltip("Punti da cui passerà la piattaforma")]
    public List<Transform> TravelPoints;

    /// <summary>
    /// Indice del waypoint da cui si deve partire
    /// </summary>
    private int fromWaypointIndex;

    /// <summary>
    /// Percentuale di spazio che manca tra i punti presi in considerazione
    /// </summary>
    private float percentBetweenWaypoints;

    /// <summary>
    /// Tempo in cui verrà eseguito il prossimo movimento
    /// </summary>
    private float nextMoveTime;

    [Header("Fading platform")]
    [SerializeField]
    /// <summary>
    /// Se true, la piattaforma può svanire
    /// </summary>
    [Tooltip("Se true, la piattaforma può svanire")]
    private bool canFade;

    [SerializeField]
    /// <summary>
    /// Tempo che ci mette la piattaforma a diventare inutilizzabile da quando ci sei sopra
    /// </summary>
    [Tooltip("Tempo che ci mette la piattaforma a diventare inutilizzabile da quando ci sei sopra")]
    private float shakingTime;

    [SerializeField]
    /// <summary>
    /// Tempo che ci mette la piattaforma a scomparire da quando è diventata inutilizzabile
    /// </summary>
    [Tooltip("Tempo che ci mette la piattaforma a scomparire da quando è diventata inutilizzabile")]
    private float fadingTime;

    [SerializeField]
    /// <summary>
    /// Tempo che ci mette la piattaforma a ritornare utilizzabile
    /// </summary>
    [Tooltip("Tempo che ci mette la piattaforma a ritornare utilizzabile")]
    private float returningTime;

    /// <summary>
    /// Se true, la piattaforma sta scomparendo
    /// </summary>
    private bool isFading;

    /// <summary>
    /// Se true, la piattaforma sta ritornando utilizzabile
    /// </summary>
    [Tooltip("Se true, la piattaforma sta ritornando utilizzabile")]
    private bool isReturning;

    [Header("Falling Platform")]
    [SerializeField]
    /// <summary>
    /// Se true, la piattaforma può cadere se colpita un numero necessario di volte
    /// </summary>
    private bool canFall;

    [SerializeField]
    /// <summary>
    /// Colpi che deve subire la piattaforma prima di iniziare a tremare
    /// </summary>
    [Tooltip("Colpi che deve subire la piattaforma prima di iniziare a tremare")]
    private int hitsNeeded;

    /// <summary>
    /// Colpi inflitti alla piattaforma
    /// </summary>
    private int currentHits;

    [SerializeField]
    /// <summary>
    /// Danno che fa la piattaforma quando cade
    /// </summary>
    [Tooltip("Danno che fa la piattaforma quando cade")]
    private int FallingDamage;

    [SerializeField]
    /// <summary>
    /// Tempo che ci mette la piattaforma a staccarsi dal soffitto
    /// </summary>
    [Tooltip("Tempo che ci mette la piattaforma a staccarsi dal soffitto")]
    private float tremblingTime;

    [SerializeField]
    /// <summary>
    /// Tempo che ci mette la piattaforma a cadere
    /// </summary>
    [Tooltip("Tempo che ci mette la piattaforma a cadere")]
    private float fallingTime;

    /// <summary>
    /// Se true, la piattaforma sta tremando
    /// </summary>
    private bool isTrembling;

    /// <summary>
    /// Se true, la piattaforma sta cadendo
    /// </summary>
    private bool isFalling;

    private void Start()
    {
        myRayCon = GetComponent<RaycastPlatform>();
        shakeTimer = new Timer();
        fadeTimer = new Timer();
        returnTimer = new Timer();
        trembleTimer = new Timer();
        fallTimer = new Timer();
    }

    private void Update()
    {
        Vector3 velocity = Move();

        myRayCon.CalculatePassengerMovement(velocity);

        myRayCon.MovePassenger(true);
        transform.Translate(velocity);
        myRayCon.MovePassenger(false);
    }

    public void ShakePlatform()
    {
        if (!shakeTimer.CheckTimer(shakingTime))
        {
            shakeTimer.TickTimer();
        }
        else
        {
            shakeTimer.StopTimer();
            myRayCon.myCollider.enabled = false;
            isFading = true;
        }
    }

    public void FadePlatform()
    {
        if (!fadeTimer.CheckTimer(fadingTime))
        {
            fadeTimer.TickTimer();
        }
        else
        {
            fadeTimer.StopTimer();
            isFading = false;
            isReturning = true;
        }
    }

    public void ReturnPlatform() // Da decidere se la piattaforma è già utilizzabile mentre sta per tornare oppure all'intera comparsa di essa
    {
        if (!returnTimer.CheckTimer(returningTime))
        {
            returnTimer.TickTimer();
        }
        else
        {
            returnTimer.StopTimer();
            myRayCon.myCollider.enabled = true;
            isReturning = false;
        }
    }

    public void TremblePlatform()
    {
        if (!trembleTimer.CheckTimer(tremblingTime))
        {
            trembleTimer.TickTimer();
        }
        else
        {
            trembleTimer.StopTimer();
            isFalling = true;
        }
    }

    public void FallPlatform()
    {
        if (!fallTimer.CheckTimer(fallingTime))
        {
            fallTimer.TickTimer();
        }
        else
        {
            fallTimer.StopTimer();
            isFalling = false;
        }
    }

    public void TakeDamage(int _takenDamage)
    {
        if (_takenDamage < 0)
        {
            return;
        }
        if (currentHits - _takenDamage <= 0)
        {
            currentHits = 0;
        }
        if (currentHits <= 0)
        {
            isTrembling = true;
        }
        else
        {
            currentHits -= _takenDamage;
        }
    }

    public bool GetCanFade()
    {
        return canFade;
    }

    public bool GetIsFading()
    {
        return isFading;
    }

    public bool GetIsReturning()
    {
        return isReturning;
    }

    public bool GetCanFall()
    {
        return canFall;
    }

    public bool GetIsFalling()
    {
        return isFalling;
    }

    public bool GetIsTrembling()
    {
        return isTrembling;
    }

    public void ResetCurrentHits()
    {
        currentHits = hitsNeeded;
    }

    private float Ease(float x)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    private Vector3 Move()
    {
        if (Time.time < nextMoveTime || (fromWaypointIndex >= TravelPoints.Count - 1 && (!Cyclic)))
        {
            return Vector3.zero;
        }

        fromWaypointIndex %= TravelPoints.Count;

        int toWaypointIndex = fromWaypointIndex + 1;

        toWaypointIndex %= TravelPoints.Count;

        float distanceBetweenWaypoints = Vector3.Distance(TravelPoints[fromWaypointIndex].position, TravelPoints[toWaypointIndex].position);

        percentBetweenWaypoints += Time.deltaTime * MovementSpeed / distanceBetweenWaypoints;

        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);

        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(TravelPoints[fromWaypointIndex].position, TravelPoints[toWaypointIndex].position, easedPercentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;

            fromWaypointIndex++;

            if (Backward)
            {
                if (fromWaypointIndex >= TravelPoints.Count - 1)
                {
                    fromWaypointIndex = 0;

                    TravelPoints.Reverse();
                }
            }
            nextMoveTime = Time.time + WaitTime;
        }
        return newPos - transform.position;
    }
}
