namespace GodHunt.StateMachine
{
    public class Game_MainMenuSetup_State : GameStateBase
    {
        public override void Enter()
        {
            context.sceneFader.SetState(SceneFader.State.FadedOut);

            FindObjectOfType<GameManager>().Init();
            GameManager.Instance.Setup();

            FindObjectOfType<InputChecker>().Setup();

            context.sceneFader.StartFade(SceneFader.State.FadedIn, 1f, () => context.OnStateEnd?.Invoke());
        }
    } 
}