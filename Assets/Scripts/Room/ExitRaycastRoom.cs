using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRaycastRoom : RaycastController
{
    [HideInInspector]
    /// <summary>
    /// Se true, il player ha superato il trigger della stanza
    /// </summary>
    public bool IsPlayerPassedThrough;

    [SerializeField]
    /// <summary>
    /// Se true, il trigger si attiva dall'alto e dal basso, altrimenti da destra e sinistra
    /// </summary>
    [Tooltip("Se true, il trigger si attiva dall'alto e dal basso, altrimenti da destra e sinistra")]
    private bool isHorizontal;

    [SerializeField]
    /// <summary>
    /// Stanze che bisogna disattivare al sorpasso di questo trigger
    /// </summary>
    [Tooltip("Stanze che bisogna disattivare al sorpasso di questo trigger")]
    private List<RoomController> roomsToDisable;

    /// <summary>
    /// Funzione che lancia dei raycasts, serve a capire quando il player esce o entra da una stanza
    /// </summary>
    public void CheckExitTrigger()
    {
        if (isHorizontal)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 rayOrigin = myRaycastOrigins.BottomLeft;
                rayOrigin += Vector2.up * (myCollider.size.y * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, myCollider.size.x, GeneralMask);

                Debug.DrawRay(rayOrigin, Vector2.right * myCollider.size.x, Color.red);

                if (hit) // Mentre colpisco qualcosa
                {
                    foreach (RoomController room in roomsToDisable)
                    {
                        room.DisableRoom();
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 rayOrigin = myRaycastOrigins.BottomLeft;
                rayOrigin += Vector2.right * (myCollider.size.x * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, myCollider.size.y, GeneralMask);

                Debug.DrawRay(rayOrigin, Vector2.up * myCollider.size.y, Color.red);

                if (hit) // Mentre colpisco qualcosa
                {
                    foreach (RoomController room in roomsToDisable)
                    {
                        room.DisableRoom();
                    }
                }
            }
        }
    }
}