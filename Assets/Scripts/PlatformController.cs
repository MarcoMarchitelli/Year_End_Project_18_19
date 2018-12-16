using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    /// <summary>
    /// Velocità con cui si muove la piattaforma
    /// </summary>
    [Tooltip("Velocità con cui si muove la piattaforma")]
    public float MovementSpeed;

    [Header("Platform behaviour")]
    /// <summary>
    /// Valore di prova per simulare vari platform
    /// </summary>
    [Tooltip("Valore di prova per simulare vari platform")]
    [Range (0,2)]
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

    private void Update()
    {
        Vector3 velocity = Move();

        transform.Translate(velocity);
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
