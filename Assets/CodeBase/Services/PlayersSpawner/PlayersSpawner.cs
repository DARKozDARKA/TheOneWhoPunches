using System.Collections.Generic;
using System.Linq;
using CodeBase.CameraScripts;
using CodeBase.Characters.Player.Logic;
using CodeBase.Characters.Player.Presenter;
using CodeBase.Infrastructure.ServiceLocator;
using CodeBase.Infrastructure.States.GameLoop;
using CodeBase.Networking.Messages;
using CodeBase.Services.ConnectionsHandlerService;
using CodeBase.Services.Factories;
using CodeBase.Services.Injection;
using CodeBase.Services.InputHandler;
using CodeBase.Services.StaticData;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace CodeBase.Services.PlayersSpawner
{
    public class PlayersSpawner : IPlayersSpawner
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly ConnectionsHandler _connectionsHandler;
        private readonly Injector _injector;
        private readonly PlayerGateaway _playerGateaway;

        private Vector3[] _allSpawnPoints;
        private List<Vector3> _avaliableSpawnPoints;
        private Dictionary<int, GameObject> _players = new();


        public PlayersSpawner(IPrefabFactory prefabFactory, IStaticDataProvider staticDataProvider,
            PlayersScore playersScore, ConnectionsHandler connectionsHandler, Injector injector)
        {
            _prefabFactory = prefabFactory;
            _staticDataProvider = staticDataProvider;
            _connectionsHandler = connectionsHandler;
            _injector = injector;
            _playerGateaway = new PlayerGateaway(playersScore);
        }

        public void LoadLevelPoints()
        {
            _allSpawnPoints = _staticDataProvider.GetLevelData(SceneManager.GetActiveScene().name).SpawnPoints;
            _avaliableSpawnPoints = _allSpawnPoints.ToList();
        }

        public void SpawnPlayerOnServer(NetworkConnection conn, PlayerServerData data)
        {
            GameObject player = CreatePlayerOnServer(conn, data.Name);

            _connectionsHandler.AddPlayerForConnection(conn, player);
            conn.Send(new SendToClientNewPlayer(player));
        }

        public void RemovePlayer(NetworkConnectionToClient conn) =>
            _players.Remove(conn.connectionId);

        public void ConstructPlayerOnClient(SendToClientNewPlayer message)
        {
            GameObject camera = Camera.main.gameObject;

            _injector.InjectIntoCamera(camera);
            _injector.InjectIntoPlayer(player: message.PlayerObject, camera: camera);
            
            camera.GetComponent<CameraFollower>().SetTarget(message.PlayerObject.gameObject);
        }

        public void DestroyAllPlayers()
        {
            for (int i = 0; i < _players.Count; i++)
                NetworkServer.Destroy(_players[_players.ElementAt(i).Key]);

            _players.Clear();
        }

        private GameObject CreatePlayerOnServer(NetworkConnection conn, string playerName)
        {
            PlayerServerData playerServerData = new PlayerServerData
            {
                ID = conn.connectionId,
                Name = playerName
            };
            GameObject player = _prefabFactory.CreatePlayer(playerServerData, GetRandomPoint());
            _playerGateaway.AddPlayer(player.GetComponent<PlayerMessenger>(), conn.connectionId);
            _players.Add(conn.connectionId, player.gameObject);
            return player;
        }


        private Vector3 GetRandomPoint()
        {
            int spawnPointIndex = Random.Range(0, _avaliableSpawnPoints.Count - 1);
            Vector3 spawnPoint = _avaliableSpawnPoints[spawnPointIndex];
            _avaliableSpawnPoints.RemoveAt(spawnPointIndex);

            if (_avaliableSpawnPoints.Count == 0)
                _allSpawnPoints.CopyTo(_avaliableSpawnPoints);

            return spawnPoint;
        }
    }
}