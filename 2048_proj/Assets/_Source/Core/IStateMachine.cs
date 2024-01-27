using Core.GameStates;

namespace Core
{
    public interface IStateMachine
    {
        public void SwitchGameState<T>() where T : AGameState;
    }
}