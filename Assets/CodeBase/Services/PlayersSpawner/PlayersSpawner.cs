using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.CameraScripts;
using CodeBase.Characters.Player;
using CodeBase.Networking.Messages;
using CodeBase.Services.Factories;
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

        private Vector3[] _allSpawnPoints;
        private List<Vector3> _avaliableSpawnPoints;
        private List<GameObject> _players = new();

        public Action<PlayerServer> OnPlayerCreated { get; set; }

        public PlayersSpawner(IPrefabFactory prefabFactory, IStaticDataProvider staticDataProvider)
        {
            _prefabFactory = prefabFactory;
            _staticDataProvider = staticDataProvider;
        }

        public void SetLevelPoints()
        {
            _allSpawnPoints = _staticDataProvider.GetLevelData(SceneManager.GetActiveScene().name).SpawnPoints;
            _avaliableSpawnPoints = _allSpawnPoints.ToList();
        }

        public void SpawnPlayerOnServer(NetworkConnection conn, string playerName)
        {
            PlayerServer player = _prefabFactory.CreatePlayer(GetRandomPoint());
            player.LoadDataFromServer(conn.connectionId, playerName);
            OnPlayerCreated?.Invoke(player);
            _players.Add(player.gameObject);
        
            NetworkServer.AddPlayerForConnection(NetworkServer.connections[conn.connectionId], player.gameObject);
            conn.Send(new SendToClientNewPlayer(player));
        }

        public void ConstructPlayerOnClient(SendToClientNewPlayer message)
        {
            Camera camera = Camera.main;
        
            camera.GetComponent<CameraFollower>().SetTarget(message.PlayerObject.gameObject);
            message.PlayerObject.GetComponent<PlayerClient>()?.ConstructOnClient(camera.gameObject);
        }

        public void DestroyAllPlayers()
        {
            _players.ForEach(NetworkServer.Destroy);
            _players.Clear();
        } 
        


        private Vector3 GetRandomPoint()
        {
            var spawnPointIndex = Random.Range(0, _avaliableSpawnPoints.Count - 1);
            var spawnPoint = _avaliableSpawnPoints[spawnPointIndex];
            _avaliableSpawnPoints.RemoveAt(spawnPointIndex);

            if (_avaliableSpawnPoints.Count == 0)
                _allSpawnPoints.CopyTo(_avaliableSpawnPoints);

            return spawnPoint;
        }
    }
}