namespace GodHunt.StateMachine
{
    public class Level_Play_State : LevelStateBase
    {
        public override void Enter()
        {
            context.backToMainMenuButton.OnClick.AddListener( () =>
                context.sceneFader.StartFade(SceneFader.State.FadedOut, 1f, () => 
                    GameManager.Instance.LoadScene("Main Menu")
                )
            );
        }
    } 
}