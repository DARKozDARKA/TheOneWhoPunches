using CodeBase.Infrastructure.States.Definition;
using CodeBase.Infrastructure.States.Lobby;
using CodeBase.Networking;
using CodeBase.Networking.Messages;
using CodeBase.Services.ConnectionsHandlerService;
using CodeBase.Services.Factories;
using CodeBase.Services.Injection;
using CodeBase.Services.Mouse;
using CodeBase.Services.PlayersSpawner;
using CodeBase.Services.SceneLoader;
using CodeBase.Services.StaticData;
using CodeBase.Services.UserData;
using CodeBase.StaticData.Strings;
using Mirror;
using UnityEngine;

namespace CodeBase.Infrastructure.States.GameLoop
{
    public class GameLoopState : IState
    {
        private readonly GameNetworkManager _gameNetworkManager;
        private readonly IMouseService _mouseService;
        private readonly IUserDataProvider _userDataProvider;
        private readonly IUIFactory _uiFactory;
        private readonly SceneLoader _sceneLoader;
        private readonly GameStateMachine _stateMachine;

        private readonly IPlayersSpawner _playersSpawner;
        private readonly PlayersScore _playersScore;


        private GameObject _endScreenUI;
        private readonly PlayerDataStorage _playerDataStorage;
        private readonly MatchPlayer _matchPlayer;


        public GameLoopState(GameStateMachine stateMachine, SceneLoader sceneLoader,
            GameNetworkManager gameNetworkManager, IPrefabFactory prefabFactory, IStaticDataProvider staticDataProvider,
            IMouseService mouseService, IUserDataProvider userDataProvider, IUIFactory uiFactory,
            ConnectionsHandler connectionsHandler, Injector injector)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameNetworkManager = gameNetworkManager;
            _mouseService = mouseService;
            _userDataProvider = userDataProvider;
            _uiFactory = uiFactory;

            _playersScore = new PlayersScore();
            _playersSpawner = new PlayersSpawner(prefabFactory, staticDataProvider, _playersScore, connectionsHandler, injector);
            _playerDataStorage = new PlayerDataStorage();
            _matchPlayer = new MatchPlayer(_gameNetworkManager, _playersSpawner, _playersScore, _playerDataStorage);
        }

        public void Enter()
        {
            RegisterListeners();
            _mouseService.LockMouse();
            _sceneLoader.Load(SceneNames.Main, OnLoaded);
        }

        public void Exit() =>
            UnregisterListeners();

        private void RegisterListeners()
        {
            NetworkServer.RegisterHandler<RequestToServerSpawnPlayer>(HandleOnServerPlayerSpawnRequest);
            NetworkClient.RegisterHandler<SendToClientsGameEnd>(ShowGameEndScreen);
            NetworkClient.RegisterHandler<SendToClientsNewGame>(HandleNewGameOnClient);
            _gameNetworkManager.OnClientStop += DisconnectFromGame;
            _gameNetworkManager.OnServerDisconnected += RemovePlayer;
            _playersSpawner.RegisterListeners();
        }

        private void UnregisterListeners()
        {
            NetworkServer.UnregisterHandler<RequestToServerSpawnPlayer>();
            NetworkClient.UnregisterHandler<SendToClientsGameEnd>();
            NetworkClient.UnregisterHandler<SendToClientsNewGame>();
            _gameNetworkManager.OnClientStop -= DisconnectFromGame;
            _gameNetworkManager.OnServerDisconnected -= RemovePlayer;
            _playersSpawner.UnregisterListeners();
        }

        private void OnLoaded()
        {
            _playersSpawner.LoadLevelPoints();
            _uiFactory.CreateHUD();
            SendToServerSpawnRequest();
        }

        private void HandleOnServerPlayerSpawnRequest(NetworkConnectionToClient conn,
            RequestToServerSpawnPlayer message)
        {
            PlayerServerData playerData = CreatePlayerServerData(conn, message);
            _matchPlayer.SpawnPlayerOnServer(conn, playerData);
        }

        private void ShowGameEndScreen(SendToClientsGameEnd message) =>
            _endScreenUI = _uiFactory.CreateEndScreen(message.WinnerName).gameObject;

        private void HandleNewGameOnClient(SendToClientsNewGame message) =>
            DestroyEndScreen();

        private void DestroyEndScreen() =>
            Object.Destroy(_endScreenUI);

        private void DisconnectFromGame() =>
            _stateMachine.Enter<LobbyState>();

        private void SendToServerSpawnRequest() =>
            NetworkClient.Send(new RequestToServerSpawnPlayer { PlayerName = _userDataProvider.UserData.PlayerName });

        private PlayerServerData CreatePlayerServerData(NetworkConnectionToClient conn,
            RequestToServerSpawnPlayer message) =>
            new PlayerServerData()
            {
                ID = conn.connectionId,
                Name = message.PlayerName
            };

        private void RemovePlayer(NetworkConnectionToClient conn)
        {
            _matchPlayer.RemovePlayer(conn);
            _playersSpawner.RemovePlayer(conn);
        }
    }
}