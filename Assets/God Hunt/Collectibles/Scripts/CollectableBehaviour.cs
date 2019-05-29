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
        playerData.playerCollectablesBehaviour.Collect();
        OnCollection.Invoke();
    }
}