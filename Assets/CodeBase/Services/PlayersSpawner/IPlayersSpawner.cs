using CodeBase.Networking.Messages;
using CodeBase.Services.Factories;
using Mirror;

namespace CodeBase.Services.PlayersSpawner
{
    public interface IPlayersSpawner
    {
        void SpawnPlayerOnServer(NetworkConnection conn, PlayerServerData data);
        void ConstructPlayerOnClient(SendToClientNewPlayer message);
        void LoadLevelPoints();
        void DestroyAllPlayers();
        void RemovePlayer(NetworkConnectionToClient conn);
    }
}