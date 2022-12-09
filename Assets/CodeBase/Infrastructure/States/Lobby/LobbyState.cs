using CodeBase.Infrastructure.States.Definition;
using CodeBase.Infrastructure.States.GameLoop;
using CodeBase.Networking;
using CodeBase.Services.Factories;
using CodeBase.Services.Lobby;
using CodeBase.Services.Mouse;
using CodeBase.Services.SceneLoader;
using CodeBase.StaticData.Strings;
using UnityEngine;

namespace CodeBase.Infrastructure.States.Lobby
{
    public class LobbyState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ILobby _lobby;
        private readonly GameNetworkManager _networkManager;
        private readonly SceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly IMouseService _mouseService;

        private GameObject _lobbyPrefab;

        public LobbyState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, GameNetworkManager networkManager,
            ILobby lobby, IUIFactory uiFactory, IMouseService mouseService)
        {
            _lobby = lobby;
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _networkManager = networkManager;
            _uiFactory = uiFactory;
            _mouseService = mouseService;
        }

        public void Enter()
        {
            _sceneLoader.Load(SceneNames.Lobby, OnLoaded);
            _mouseService.ConfineMouse();
        }

        public void Exit() => 
            UnregisterListeners();

        private void OnLoaded()
        {
            RegisterListeners();
            _uiFactory.CreateUIRoot();
            CreateLobby();
            _lobby.CreateLobbySelector();
        }

        private void RegisterListeners() => 
            _networkManager.OnClientConnected += EnterGameState;

        private void UnregisterListeners() => 
            _networkManager.OnClientConnected -= EnterGameState;

        private void EnterGameState() => 
            _gameStateMachine.Enter<GameLoopState>();

        private void CreateLobby()
        {
            if (_lobbyPrefab != null)
                return;

            _lobbyPrefab = _uiFactory.CreateLobbySelector().gameObject;
        }
    }
}