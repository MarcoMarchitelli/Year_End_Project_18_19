namespace GodHunt.StateMachine
{
    using UnityEngine;

    public class LevelStateMachineController : BaseStateMachine
    {
        [SerializeField] LevelStateMachineContext context;

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
    public class LevelStateMachineContext : IContext
    {
        public SceneFader sceneFader;
        public DeathScreen deathScreen;
        public PlayerHPUI playerHPUI;

        public System.Action OnStateEnd;
    }
}