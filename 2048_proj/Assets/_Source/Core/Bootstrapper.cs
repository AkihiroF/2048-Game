using Core.GameStates;
using UnityEngine;
using Zenject;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [Inject] 
        private IStateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine.SwitchGameState<InitState>();
        }

        private void Start()
        {
            _stateMachine.SwitchGameState<PlayState>();
        }
    }
}