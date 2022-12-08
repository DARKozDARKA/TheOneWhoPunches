namespace CodeBase.Infrastructure.States.Definition
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}