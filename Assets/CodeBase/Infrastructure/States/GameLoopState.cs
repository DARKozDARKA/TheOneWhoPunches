using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Characters.Player;
using CodeBase.Networking;
using CodeBase.Networking.Messages;
using CodeBase.Services.ApplicationRunner;
using CodeBase.Services.Factories;
using CodeBase.Services.Mouse;
using CodeBase.Services.PlayersSpawner;
using CodeBase.Services.StaticData;
using CodeBase.Services.UserData;
using CodeBase.StaticData.Strings;
using CodeBase.StaticData.Values;
using Mirror;
using UnityEngine;

namespace CodeBase.Infrastructure.States
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

        private bool _isGameEnded;
        private GameObject _endScreenUI;
        private Dictionary<NetworkConnectionToClient, string> _playersData = new();

        public GameLoopState(GameStateMachine stateMachine, SceneLoader sceneLoader,
            GameNetworkManager gameNetworkManager, IPrefabFactory prefabFactory, IStaticDataProvider staticDataProvider,
            IMouseService mouseService, IUserDataProvider userDataProvider, IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameNetworkManager = gameNetworkManager;
            _mouseService = mouseService;
            _userDataProvider = userDataProvider;
            _uiFactory = uiFactory;

            _playersSpawner = new PlayersSpawner(prefabFactory, staticDataProvider);
        }

        public void Enter()
        {
            RegisterListeners();
            _sceneLoader.Load(SceneNames.Main, OnLoaded);
            _mouseService.LockMouse();
            _isGameEnded = false;
        }

        public void Exit() => 
            UnregisterListeners();

        private void RegisterListeners()
        {
            NetworkServer.RegisterHandler<RequestToServerSpawnPlayer>(HandlePlayerSpawnRequest);
            NetworkClient.RegisterHandler<SendToClientNewPlayer>(_playersSpawner.ConstructPlayerOnClient);
            NetworkClient.RegisterHandler<SendToClientsGameEnd>(ShowGameEndScreen);
            NetworkClient.RegisterHandler<SendToClientsNewGame>(HandleNewGameOnClient);
            _playersSpawner.OnPlayerCreated += RegisterPlayer;
            _gameNetworkManager.OnClientStop += DisconnectFromGame;
        }

        private void UnregisterListeners()
        {
            NetworkServer.UnregisterHandler<RequestToServerSpawnPlayer>();
            NetworkClient.UnregisterHandler<SendToClientNewPlayer>();
            NetworkClient.UnregisterHandler<SendToClientsGameEnd>();
            NetworkClient.UnregisterHandler<SendToClientsNewGame>();
            _playersSpawner.OnPlayerCreated -= RegisterPlayer;
            _gameNetworkManager.OnClientStop -= DisconnectFromGame;
        }

        private void OnLoaded()
        {
            _playersSpawner.SetLevelPoints();
            SendToServerSpawnRequest();
        }

        private void SendToServerSpawnRequest() =>
            NetworkClient.Send(new RequestToServerSpawnPlayer { PlayerName = _userDataProvider.UserData.PlayerName });

        private void RegisterPlayer(PlayerServer playerServer) => 
            playerServer.OnScoreChanged += HandlePlayerScoreChanged;

        private void HandlePlayerScoreChanged(PlayerServer player, int score)
        {
            if (_isGameEnded)
                return;
            
            if (CheckIfPlayerWon(score))
                EndGame(player);
        }

        private void HandlePlayerSpawnRequest(NetworkConnectionToClient conn, RequestToServerSpawnPlayer message)
        {
            _playersSpawner.SpawnPlayerOnServer(conn, message.PlayerName);
            _playersData.Add(conn, message.PlayerName);
        }


        private async void EndGame(PlayerServer winnerPlayer)
        {
            _isGameEnded = true;
            _playersSpawner.DestroyAllPlayers();
            NetworkServer.SendToAll(new SendToClientsGameEnd { WinnerName = winnerPlayer.PlayerName });
            
            await Task.Delay(GameConstants.RematchTimeInMilliseconds);
            
            StartNewGame();
        }

        private void StartNewGame()
        {
            _isGameEnded = false;
            foreach (NetworkConnectionToClient conn in _gameNetworkManager.ConnectionToClients)
            {
                _playersSpawner.SpawnPlayerOnServer(conn, _playersData[conn]);
                conn.Send(new SendToClientsNewGame());
            }
        }

        private void ShowGameEndScreen(SendToClientsGameEnd message) =>
            _endScreenUI = _uiFactory.CreateEndScreen(message.WinnerName).gameObject;

        private void HandleNewGameOnClient(SendToClientsNewGame message) =>
            DestroyEndScreen();

        private void DestroyEndScreen() =>
            Object.Destroy(_endScreenUI);

        private void DisconnectFromGame()
        {
            _stateMachine.Enter<LobbyState>();
        }
            

        private bool CheckIfPlayerWon(int score) =>
            score >= GameConstants.WinnerScore;
    }
}