using UI;
using Zenject;

namespace Core.GameStates
{
    public class FinishState : AGameState
    {
        [Inject]
        public FinishState(PlayerInput input, FinishView view)
        {
            _input = input;
            _view = view;
        }

        private readonly PlayerInput _input;
        private readonly FinishView _view;

        public override void Enter()
        {
            _input.Disable();
            _view.EnableFinishPanel();
        }
    }
}