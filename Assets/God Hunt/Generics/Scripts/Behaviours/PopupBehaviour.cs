using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PopupBehaviour : BaseBehaviour
{
    [Header("References")]
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;

    [Header("Parameters")]
    [SerializeField] Sprite imageToDisplay;
    [Multiline]
    [SerializeField] string textToDisplay;
    [SerializeField] float animationDuration;
    [SerializeField] float waitDuration;

    protected override void CustomSetup()
    {
        image.sprite = imageToDisplay;
        image.rectTransform.localScale = Vector3.zero;
        text.text = textToDisplay;
        text.rectTransform.localScale = Vector3.zero;
    }

    public void Popup()
    {
        image.rectTransform.DOScale(1, animationDuration).SetEase(Ease.OutCubic);
        if (waitDuration <= 0)
            text.rectTransform.DOScale(1, animationDuration).SetEase(Ease.OutCubic);
        else
            text.rectTransform.DOScale(1, animationDuration).SetEase(Ease.OutCubic).onComplete += () => TimerGod.Timer(waitDuration, () => Popdown());
    }

    public void Popdown()
    {
        image.rectTransform.DOScale(0, animationDuration).SetEase(Ease.OutCubic);
        text.rectTransform.DOScale(0, animationDuration).SetEase(Ease.OutCubic);
    }
}