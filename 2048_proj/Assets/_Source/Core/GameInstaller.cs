using System;
using BlockSystem;
using Core;
using Core.GameStates;
using Grid;
using Input;
using UI;
using UnityEngine;
using Zenject;

namespace _Source.Core
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GridGenerator grid;
        [SerializeField] private Bootstrapper bootstrapper;
        [SerializeField] private FinishView finishView;
        public override void InstallBindings()
        {
            Container.Bind<FinishView>().FromInstance(finishView);
            BindGrid();
            BindInput();
            BindGameStates();
            Container.Bind<Bootstrapper>().FromInstance(bootstrapper);
        }

        private void BindGrid()
        {
            Container.Bind<GridGenerator>().FromInstance(grid);
            Container.Bind<GridStateController>().AsSingle();
        }

        private void BindInput()
        {
            Container.Bind<PlayerInput>().AsSingle();
            Container.Bind<InputHandler>().AsSingle();
        }

        private void BindGameStates()
        {
            Container.Bind<AGameState>().To<InitState>().AsSingle();
            Container.Bind<AGameState>().To<PlayState>().AsSingle();
            Container.Bind<AGameState>().To<FinishState>().AsSingle();
            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
        }
    }
}