using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public float BGfadeDuration;
    public float imageFadeDuration;
    public float screenDuration;
    public float startDelay;

    System.Action OnDeathScreenEnd, OnFadeIn;

    [Header("UI References")]
    public Image BG;
    public Image image;

    int fadesCount = 0;
    PlayerEntityData playerData;

    private void Start()
    {
        playerData = GameManager.Instance.player.Data as PlayerEntityData;
        if (playerData != null)
        {
            playerData.damageReceiverBehaviour.OnHealthDepleated.AddListener(() => TimerGod.Timer(startDelay, () => StartFade() ));
            OnFadeIn += () => playerData.respawnBehaviour.Respawn(true);
            OnDeathScreenEnd += () => playerData.playerInputBehaviour.Enable(true);
            OnDeathScreenEnd += () => playerData.playerKnockbackReceiverBehaviour.Enable(true);
        }
    }

    private void OnDisable()
    {
        if (playerData != null)
        {
            playerData.damageReceiverBehaviour.OnHealthDepleated.RemoveListener(StartFade);
        }
    }

    public void StartFade()
    {
        Sequence fadeInSequence = DOTween.Sequence();

        fadeInSequence.Append(BG.DOColor(Color.black, BGfadeDuration));
        fadeInSequence.Join(image.DOColor(Color.white, imageFadeDuration));
        fadeInSequence.AppendInterval(screenDuration);
        fadeInSequence.onComplete += () =>
        {
            OnFadeIn?.Invoke();
            Sequence fadeOutSequence = DOTween.Sequence();
            fadeOutSequence.Append(BG.DOColor(Color.clear, BGfadeDuration));
            fadeInSequence.Join(image.DOColor(Color.clear, imageFadeDuration));
            fadeOutSequence.onComplete += () => OnDeathScreenEnd?.Invoke();
        };
    }
}