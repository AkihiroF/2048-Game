using Input;
using Zenject;

namespace Core.GameStates
{
    public class InitState : AGameState
    {
        [Inject]
        public InitState(PlayerInput input, InputHandler handler)
        {
            _input = input;
            _handler = handler;
        }

        private readonly PlayerInput _input;
        private readonly InputHandler _handler;

        public override void Enter()
        {
            _input.Player.Move.performed += _handler.OnMove;
        }
    }
}