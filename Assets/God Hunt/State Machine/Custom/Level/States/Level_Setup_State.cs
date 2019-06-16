namespace GodHunt.StateMachine
{
    public class Level_Setup_State : LevelStateBase
    {
        public override void Enter()
        {
            FindObjectOfType<GameManager>().Init();
            GameManager.Instance.Setup();

            FindObjectOfType<InputChecker>().Setup();

            context.deathScreen.Setup();

            PlayerEntityData playerData = GameManager.Instance.player.Data as PlayerEntityData;

            context.playerHPUI.damageReceiver = playerData.damageReceiverBehaviour;
            context.playerHPUI.Setup();

            context.sceneFader.StartFade(SceneFader.State.FadedIn, 1f);
        }
    } 
}