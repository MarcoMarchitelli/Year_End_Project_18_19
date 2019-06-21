namespace GodHunt.StateMachine
{
    using UnityEngine;
    using UnityEngine.UI;

    public class GameStateMachineController : BaseStateMachine
    {
        [SerializeField] GameStateMachineContext context;

        protected override void ContextSetup()
        {
            context.OnStateEnd += HandleStateEnd;

            CurrentContext = context;
        }

        private void HandleStateEnd()
        {
            Data.SetTrigger("Forward");
        }
    }

    [System.Serializable]
    public class GameStateMachineContext : IContext
    {
        public SceneFader sceneFader;
        public Button mainMenuPlayButton;

        public System.Action OnStateEnd;
    }
}