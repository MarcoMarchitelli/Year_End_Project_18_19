namespace GodHunt.StateMachine
{
    public class Game_MainMenu_State : GameStateBase
    {
        public override void Enter()
        {
            context.mainMenuPlayButton.OnClick.AddListener(() =>
                context.sceneFader.StartFade(SceneFader.State.FadedOut, 1f, () =>
                    { GameManager.Instance.LoadScene("Tutorial"); context.OnStateEnd?.Invoke(); }
                )
            );
        }
    } 
}