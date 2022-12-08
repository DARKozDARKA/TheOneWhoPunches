namespace CodeBase.Infrastructure.States.Definition
{
    public interface IPayloadedState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }
}