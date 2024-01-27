using Zenject;

namespace Core.GameStates
{
    public class PlayState : AGameState
    {
        private readonly PlayerInput _input;

        [Inject]
        public PlayState(PlayerInput input)
        {
            _input = input;
        }

        public override void Enter()
        {
            _input.Player.Enable();
        }
    }
}