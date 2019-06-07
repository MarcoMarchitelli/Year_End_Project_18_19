using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using System.Linq;

/// <summary>
/// Classe che fornisce le informazioni sullo stato dei gamepad collegati.
/// </summary>
public class InputChecker : MonoBehaviour
{
    #region Delegates
    public static Action<IntellGamePad> OnGamepadConnected;
    public static Action<IntellGamePad> OnGamepadDisconnected;
    #endregion

    #region properties
    /// <summary>
    /// Lista dei gamepad attivi.
    /// </summary>
    public List<IntellGamePad> Activegamepads { get; protected set; }
    #endregion

    public static InputChecker instance;

    const float CONTROLLER_STATUS_CHECK_INTERVAL = .5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Activegamepads = new List<IntellGamePad>();
        }

        StartCoroutine("ControllerStatusCheckRoutine");
    }

    IEnumerator ControllerStatusCheckRoutine()
    {
        DoCheckInput();
        yield return new WaitForSecondsRealtime(CONTROLLER_STATUS_CHECK_INTERVAL);
    }

    /// <summary>
    /// Aggiorna la lista dello stato dei gamepad (Activegamepads).
    /// </summary>
    /// <returns></returns>
    private void DoCheckInput()
    {
        for (int i = 0; i < 4; ++i)
        {
            PlayerIndex testPlayerIndex = (PlayerIndex)i;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected)
            {
                IntellGamePad newPad = Activegamepads.FirstOrDefault(gpad => gpad.ID == i);
                if (newPad != null)
                {
                    newPad.CurrentGamePadState = testState;
                }
                else
                {
                    IntellGamePad padToAdd = new IntellGamePad(testState, i);
                    Activegamepads.Add(padToAdd);
                    OnGamepadConnected?.Invoke(padToAdd);
                }
            }
            else
            {
                IntellGamePad padToRemove = Activegamepads.FirstOrDefault(gpad => gpad.ID == i);
                if (padToRemove != null)
                {
                    Activegamepads.RemoveAt(i);
                    OnGamepadDisconnected?.Invoke(padToRemove);
                }
            }
        }
    }
}