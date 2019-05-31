public class UnlockableSkillBehaviour : BaseBehaviour
{
    public enum SkillType { dash, sprint, doubleJump }

    public SkillType skillToUnlock;

    PlayerEntityData playerData;

    protected override void CustomSetup()
    {
        playerData = GameManager.Instance.player.Data as PlayerEntityData;
    }

    public void UnlockSkill()
    {
        switch (skillToUnlock)
        {
            case SkillType.dash:
                playerData.playerInputBehaviour.SetDashInput(true);
                break;
            case SkillType.sprint:
                playerData.playerInputBehaviour.SetRunInput(true);
                break;
            case SkillType.doubleJump:
                playerData.playerGameplayBehaviour.ToggleDoubleJump(true);
                break;
        }
    }
}