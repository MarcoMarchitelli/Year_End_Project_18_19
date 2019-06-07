using UnityEngine;

public class CollectableBehaviour : BaseBehaviour
{
    [SerializeField] UnityVoidEvent OnCollection;

    PlayerEntityData playerData;

    protected override void CustomSetup()
    {
        playerData = GameManager.Instance.player.Data as PlayerEntityData;
    }

    public void Collect()
    {
        if (!isEnabled)
            return;

        playerData.playerCollectablesBehaviour.Collect();
        OnCollection.Invoke();
    }
}