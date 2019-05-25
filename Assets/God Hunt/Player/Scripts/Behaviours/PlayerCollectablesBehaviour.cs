using UnityEngine;

public class PlayerCollectablesBehaviour : BaseBehaviour
{
    const int COLLECTABLES_TO_UPGRADE = 2;

    PlayerEntityData data;
    int collectablesCount;

    protected override void CustomSetup()
    {
        data = Entity.Data as PlayerEntityData;

        //should read from playerprefs!
        collectablesCount = 0;
    }

    public void Collect()
    {
        collectablesCount++;
        if(collectablesCount >= COLLECTABLES_TO_UPGRADE)
        {
            data.damageReceiverBehaviour.UpgradeHealth();
            collectablesCount = 0;
        }
    }
}