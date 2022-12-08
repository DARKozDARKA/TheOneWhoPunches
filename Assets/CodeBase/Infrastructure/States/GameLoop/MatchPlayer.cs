using System.Threading.Tasks;
using CodeBase.Networking;
using CodeBase.Networking.Messages;
using CodeBase.Services.Factories;
using CodeBase.Services.PlayersSpawner;
using CodeBase.StaticData.Values;
using Mirror;

namespace CodeBase.Infrastructure.States.GameLoop
{
    public class MatchPlayer
    {
        private readonly GameNetworkManager _gameNetworkManager;
        private readonly IPlayersSpawner _playersSpawner;
        private readonly PlayersScore _playersScore;
        private readonly PlayerDataStorage _playerDataStorage;

        private bool _isGameEnded;
        
        public MatchPlayer(GameNetworkManager gameNetworkManager, IPlayersSpawner playersSpawner, PlayersScore playerScore, PlayerDataStorage playerDataStorage)
        {
            _gameNetworkManager = gameNetworkManager;
            _playersSpawner = playersSpawner;
            _playersScore = playerScore;
            _playerDataStorage = playerDataStorage;
            
            _playersScore.OnScoreChanged += HandleOnServerPlayerScoreChanged;
        }
        
        ~ MatchPlayer()
        {
            _playersScore.OnScoreChanged -= HandleOnServerPlayerScoreChanged;
        }
        
        public void SpawnPlayerOnServer(NetworkConnectionToClient conn, PlayerServerData playerData)
        {
            _playersSpawner.SpawnPlayerOnServer(conn, playerData);
            _playersScore.AddPlayer(conn.connectionId);
            _playerDataStorage.AddPlayerData(conn.connectionId, playerData);
        }

        public void RemovePlayer(NetworkConnectionToClient conn)
        {
            _playerDataStorage.RemovePlayerData(conn.connectionId);
            _playersScore.RemovePlayer(conn.connectionId);
        }
        
        private void CreatePlayers()
        {
            foreach (NetworkConnectionToClient conn in _gameNetworkManager.ConnectionToClients)
            {
                _playersSpawner.SpawnPlayerOnServer(conn, _playerDataStorage.GetData(conn.connectionId));
                conn.Send(new SendToClientsNewGame());
            }
        }

        private void HandleOnServerPlayerScoreChanged(int ID, int score)
        {
            if (_isGameEnded)
                return;
            
            if (CheckIfPlayerWon(score))
                EndGame(_playerDataStorage.GetData(ID));
        }
        
        private async void EndGame(PlayerServerData winnerData)
        {
            _isGameEnded = true;
            _playersSpawner.DestroyAllPlayers();
            NetworkServer.SendToAll(new SendToClientsGameEnd { WinnerName = winnerData.Name });
            
            await Task.Delay(GameConstants.RematchTimeInMilliseconds);
            
            StartNewGame();
        }

        private void StartNewGame()
        {
            _isGameEnded = false;
            _playersScore.ResetScore();
            CreatePlayers();
        }

        private bool CheckIfPlayerWon(int score) =>
            score >= GameConstants.WinnerScore;
    }
}