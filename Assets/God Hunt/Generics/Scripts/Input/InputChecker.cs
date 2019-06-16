using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using System.Linq;

public class InputChecker : MonoBehaviour
{
    public static Action<IntellGamePad> OnGamepadConnected;
    public static Action<IntellGamePad> OnGamepadDisconnected;
    public static InputChecker instance;

    public List<IntellGamePad> Activegamepads = new List<IntellGamePad>();

    const float CONTROLLER_STATUS_CHECK_INTERVAL = .5f;

    public void Setup()
    {
        if (instance == null)
        {
            instance = this;
        }

        StartCoroutine("ControllerStatusCheckRoutine");
    }

    private void Update()
    {
        UpdatePadsInputStatus();
    }

    IEnumerator ControllerStatusCheckRoutine()
    {
        while (true)
        {
            UpdatePadsConnectionStatus();
            yield return new WaitForSecondsRealtime(CONTROLLER_STATUS_CHECK_INTERVAL);
        }
    }

    /// <summary>
    /// Updates pads connection and disconnection.
    /// </summary>
    /// <returns></returns>
    private void UpdatePadsConnectionStatus()
    {
        for (int i = 0; i < 4; ++i)
        {
            GamePadState testState = GamePad.GetState((PlayerIndex)i);

            if (testState.IsConnected)
            {
                IntellGamePad newPad = Activegamepads.FirstOrDefault(gpad => gpad.ID == i);
                if (newPad == null)
                {
                    IntellGamePad padToAdd = new IntellGamePad(testState, i);
                    Activegamepads.Add(padToAdd);
                    OnGamepadConnected?.Invoke(padToAdd);
                    Debug.Log("CONTROLLER SIIII");
                }
            }
            else
            {
                IntellGamePad padToRemove = Activegamepads.FirstOrDefault(gpad => gpad.ID == i);
                if (padToRemove != null)
                {
                    Activegamepads.RemoveAt(i);
                    OnGamepadDisconnected?.Invoke(padToRemove);
                    Debug.Log("TASTIERA SEEEE");
                }
            }
        }
    }

    private void UpdatePadsInputStatus()
    {
        foreach (IntellGamePad pad in Activegamepads)
        {
            pad.CurrentGamePadState = GamePad.GetState((PlayerIndex)pad.ID);
        }
    }
}