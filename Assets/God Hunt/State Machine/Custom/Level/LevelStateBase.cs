namespace GodHunt.StateMachine
{
    public abstract class LevelStateBase : BaseState
    {
        protected LevelStateMachineContext context;

        public override void Setup(IContext _context)
        {
            context = _context as LevelStateMachineContext;
        }
    }
}