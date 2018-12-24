using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer {

    private float timer = 0f;

    public void TickTimer()
    {
        timer += Time.deltaTime;
    }

    public void PauseTimer()
    {
        timer += 0f;
    }

    public void StopTimer()
    {
        timer = 0f;
    }

    public bool CheckTimer(float timerToCheck)
    {
        if (timer >= timerToCheck)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
