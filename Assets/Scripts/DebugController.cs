using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    private Canvas myCanvas;

    private void Start()
    {
        myCanvas = GetComponentInChildren<Canvas>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchCanvasStatus();
        }
    }

    private void SwitchCanvasStatus()
    {
        myCanvas.enabled = !myCanvas.enabled;
        Time.timeScale = myCanvas.enabled ? 0 : 1;
    }
}
