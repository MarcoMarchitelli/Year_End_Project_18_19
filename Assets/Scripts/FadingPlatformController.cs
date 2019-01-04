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

    [SerializeField]
    /// <summary>
    /// Tempo che ci mette la piattaforma a diventare inutilizzabile da quando ci sei sopra
    /// </summary>
    [Tooltip("Tempo che ci mette la piattaforma a diventare inutilizzabile da quando ci sei sopra")]
    private float fadingTime;

    [SerializeField]
    /// <summary>
    /// Tempo che ci mette la piattaforma a ritornare utilizzabile
    /// </summary>
    [Tooltip("Tempo che ci mette la piattaforma a ritornare utilizzabile")]
    private float returningTime;
}
