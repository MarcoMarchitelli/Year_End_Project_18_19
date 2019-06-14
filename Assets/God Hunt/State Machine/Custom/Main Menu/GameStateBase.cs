namespace GodHunt.StateMachine
{
    public abstract class GameStateBase : BaseState
    {
        protected GameStateMachineContext context;

        public override void Setup(IContext _context)
        {
            context = _context as GameStateMachineContext;
        }
    }
}