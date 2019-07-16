using UnityEngine;
using DG.Tweening;

public class AudioFader : MonoBehaviour
{
    public AudioSource source;
    public float fadeDuration;
    public UnityVoidEvent OnFadeInStart, OnFadeInEnd, OnFadeOutStart, OnFadeOutEnd;

    public void FadeOut()
    {
        OnFadeOutStart.Invoke();
        source.DOFade( 0, fadeDuration ).onComplete += () => OnFadeOutEnd.Invoke();
    }

    public void FadeIn()
    {
        OnFadeInStart.Invoke();
        source.DOFade( 1, fadeDuration ).onComplete += () => OnFadeInEnd.Invoke();
    }
}