using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.ServiceLocator;
using CodeBase.Infrastructure.States.Bootstrap;
using CodeBase.Infrastructure.States.Definition;
using CodeBase.Infrastructure.States.GameLoop;
using CodeBase.Infrastructure.States.Lobby;
using CodeBase.Networking;
using CodeBase.Services.ConnectionsHandlerService;
using CodeBase.Services.Factories;
using CodeBase.Services.Lobby;
using CodeBase.Services.Mouse;
using CodeBase.Services.SceneLoader;
using CodeBase.Services.StaticData;
using CodeBase.Services.UserData;

namespace CodeBase.Infrastructure
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, AllServices services, GameNetworkManager networkManager)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] =
                    new BootstrapState(this, sceneLoader, networkManager, services),

                [typeof(LobbyState)] = new LobbyState(this, sceneLoader, networkManager,
                    services.Single<ILobby>(), services.Single<IUIFactory>(), services.Single<IMouseService>()),

                [typeof(GameLoopState)] =
                    new GameLoopState(this, sceneLoader, networkManager, services.Single<IPrefabFactory>(),
                        services.Single<IStaticDataProvider>(), services.Single<IMouseService>(),
                        services.Single<IUserDataProvider>(), services.Single<IUIFactory>(),
                        services.Single<ConnectionsHandler>())
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            var state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}