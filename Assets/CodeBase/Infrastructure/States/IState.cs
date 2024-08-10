namespace CodeBase.Infrastructure.States
{
    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface IPayloadedState<TPalyload> :IExitableState
    {
        void Enter(TPalyload payload);
    }

    public interface IExitableState
    {
        void Exit();
    }
}