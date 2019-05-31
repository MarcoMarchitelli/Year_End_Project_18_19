using UnityEngine;
using DG.Tweening;

public class ScalerBehaviour : BaseBehaviour
{
    public Vector3 startScale;
    public Vector3 endScale;
    public float duration;
    public UnityVoidEvent OnScaleStart, OnScaleEnd;

    public void ScaleThisBehaviour()
    {
        OnScaleStart.Invoke();
        transform.localScale = startScale;
        transform.DOScale(endScale, duration).onComplete += () => OnScaleEnd.Invoke();
    }

    public void ScaleEntity()
    {
        OnScaleStart.Invoke();
        Entity.gameObject.transform.localScale = startScale;
        Entity.gameObject.transform.DOScale(endScale, duration).onComplete += () => OnScaleEnd.Invoke();
    }
}