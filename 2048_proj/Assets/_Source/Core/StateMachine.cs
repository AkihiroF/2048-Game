using System;
using System.Collections.Generic;
using BlockSystem;
using Core.GameStates;
using Zenject;

namespace Core
{
    public class StateMachine : IStateMachine
    {
        private readonly Dictionary<Type, AGameState> _states;
        private AGameState _currentState;

        [Inject]
        public StateMachine(List<AGameState> states)
        {
            _states = new Dictionary<Type, AGameState>();
            foreach (var state in states)
            {
                var typeState = state.GetType();
                _states.Add(typeState,state);
            }

            GridStateController.OnFinish += OnSwitchToFinishState;
        }
        
        public void SwitchGameState<T>() where T : AGameState
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            if (_states.TryGetValue(typeof(T), out AGameState newState))
            {
                _currentState = newState;
                _currentState.Enter();
            }
            else
            {
                throw new Exception($"State {typeof(T).Name} not found.");
            }
        }

        private void OnSwitchToFinishState()
        {
            GridStateController.OnFinish -= OnSwitchToFinishState;
            SwitchGameState<FinishState>();
        }

    }
}