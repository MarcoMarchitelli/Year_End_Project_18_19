using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastPlatform))]
public class FadingPlatformController : MonoBehaviour
{
    /// <summary>
    /// Riferimento allo script RaycastController della piattaforma
    /// </summary>
    public RaycastPlatform myRayCon;

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
        myRayCon.Collisions.ResetCollisionInfo();
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
}
