using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public bool autoStart = true;
    public Image fadeImage;

    public void Awake()
    {
        if (autoStart)
        {
            FadeIn(.5f);
        }
    }

    public void FadeIn(float _duration)
    {
        StartCoroutine(Fade(_duration, 0, 1));
    }

    public void FadeOut(float _duration)
    {

    }

    IEnumerator Fade(float _duration, float _from, float _to)
    {
        yield return null;
    }
}