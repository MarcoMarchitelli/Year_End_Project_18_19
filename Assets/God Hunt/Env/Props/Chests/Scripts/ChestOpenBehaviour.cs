using UnityEngine;

public class ChestOpenBehaviour : BaseBehaviour
{
    Animator anim;
    PlayerEntityData data;

    protected override void CustomSetup()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void OpenChest()
    {
        anim.SetTrigger("Open");
        data = GameManager.Instance.player.Data as PlayerEntityData;
        data.playerCollectablesBehaviour.Collect();
    }
}