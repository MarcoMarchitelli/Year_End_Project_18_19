using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastPlatform))]
public class PlatformController : MonoBehaviour
{
    /// <summary>
    /// Riferimento allo script RaycastController della piattaforma
    /// </summary>
    public RaycastPlatform myRayCon;

    /// <summary>
    /// Velocità con cui si muove la piattaforma
    /// </summary>
    [Tooltip("Velocità con cui si muove la piattaforma")]
    public float MovementSpeed;

    [Header("Moving Platform")]
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

    [Header("Fading platform")]
    [SerializeField]
    /// <summary>
    /// Se true, la piattaforma può svanire
    /// </summary>
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
    [Tooltip("Se true, la piattaforma sta scomparendo")]
    public bool isFading;

    /// <summary>
    /// Se true, la piattaforma sta ritornando utilizzabile
    /// </summary>
    [Tooltip("Se true, la piattaforma sta ritornando utilizzabile")]
    public bool isReturning;

    private void Start()
    {
        myRayCon = GetComponent<RaycastPlatform>();
        shakeTimer = new Timer();
        fadeTimer = new Timer();
        returnTimer = new Timer();
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

    public bool GetCanFade()
    {
        return canFade;
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
