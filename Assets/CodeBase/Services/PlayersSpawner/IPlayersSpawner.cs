using System;
using CodeBase.Characters.Player;
using CodeBase.Networking.Messages;
using Mirror;

namespace CodeBase.Services.PlayersSpawner
{
    public interface IPlayersSpawner
    {
        void SpawnPlayerOnServer(NetworkConnection conn, string playerName);
        void ConstructPlayerOnClient(SendToClientNewPlayer message);
        void SetLevelPoints();
        Action<PlayerServer> OnPlayerCreated { get; set; }
        void DestroyAllPlayers();
    }
}