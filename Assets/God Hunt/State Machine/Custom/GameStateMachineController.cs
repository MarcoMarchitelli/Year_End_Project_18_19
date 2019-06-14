namespace GodHunt.StateMachine
{
    using UnityEngine;

    public class GameStateMachineController : BaseStateMachine
    {
        [SerializeField] GameStateMachineContext context;

        protected override void CustomSetup()
        {
            DontDestroyOnLoad(this);
        }

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

        public System.Action OnStateEnd;
    }
}