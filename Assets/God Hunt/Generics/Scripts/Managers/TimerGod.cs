using System.Collections;
using UnityEngine;
using System;

public class TimerGod : MonoBehaviour
{
    public static TimerGod Instance;

    private void Awake()
    {
        if( Instance == null)
        {
            Instance = this;
        }
    }

    public static void Timer(float _time, Action _callback)
    {
        Instance.StartCoroutine(Instance.CountTime(_time, _callback));
    }

    IEnumerator CountTime(float _time, Action _callback)
    {
        yield return new WaitForSeconds(_time);
        _callback?.Invoke();
    }
}