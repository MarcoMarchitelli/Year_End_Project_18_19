using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField]
    /// <summary>
    /// Se true, il player parte da questa stanza
    /// </summary>
    [Tooltip("Se true, il player parte da questa stanza")]
    private bool isFirstRoom;

    [SerializeField]
    [Tooltip("ChillRoom: Stanza normale, è aperta e viene chiusa solo dopo essere entrati nella seconda stanza dopo questa (Tranne se quest'ultima è OpenRoom) \n \n" +
        "AggressiveRoom: Stanza normale, è aperta ma viene chiusa appena si entra nella prossima stanza (Tranne se quest'ultima è OpenRoom) \n \n" +
        "OpenRoom: Stanza speciale, rimane sempre aperta e non chiude alcuna stanza \n \n" +
        "FightRoom: Stanza speciale, è aperta ma viene chiusa in tutte le direzioni non appena si entra in essa, si riapre dopo un evento interno alla stanza")]
    private RoomType roomType;

    /// <summary>
    /// Lista di EnterRaycast di questa stanza
    /// </summary>
    [Tooltip("Lista di EnterRaycast di questa stanza")]
    public List<EnterRaycastRoom> myEnterRaycasts;

    /// <summary>
    /// Lista di ExitRaycast di questa stanza
    /// </summary>
    [Tooltip("Lista di ExitRaycast di questa stanza")]
    public List<ExitRaycastRoom> myExitRaycasts;

    [HideInInspector]
    /// <summary>
    /// Se true, il player è in questa stanza
    /// </summary>
    public bool IsPlayerInThisRoom;

    private void Start()
    {
        if (isFirstRoom)
        {
            SetRoomBool(true);
        }
    }

    public void EnableRoom()
    {
        GetComponentsInChildren<Transform>(true)[1].gameObject.SetActive(true);
        SetRoomBool(true);
    }

    public void DisableRoom()
    {
        GetComponentsInChildren<Transform>(true)[1].gameObject.SetActive(false);
        SetRoomBool(false);
    }

    public void SetRoomBool(bool value)
    {
        IsPlayerInThisRoom = value;
    }

    public bool GetRoomBool()
    {
        return IsPlayerInThisRoom;
    }
}

public enum RoomType
{
    /// <summary>
    /// Stanza normale, è aperta e viene chiusa solo dopo essere entrati nella seconda stanza dopo questa (Tranne se quest'ultima è OpenRoom)
    /// </summary>
    ChillRoom = 0,

    /// <summary>
    /// Stanza normale, è aperta ma viene chiusa appena si entra nella prossima stanza (Tranne se quest'ultima è OpenRoom)
    /// </summary>
    AggressiveRoom = 1,

    /// <summary>
    /// Stanza speciale, rimane sempre aperta e non chiude alcuna stanza
    /// </summary>
    OpenRoom = 2,

    /// <summary>
    /// Stanza speciale, è aperta ma viene chiusa in tutte le direzioni non appena si entra in essa
    /// </summary>
    FightRoom = 3
}