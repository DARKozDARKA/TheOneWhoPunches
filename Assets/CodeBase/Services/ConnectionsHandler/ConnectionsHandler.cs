using CodeBase.Infrastructure.ServiceLocator;
using Mirror;
using UnityEngine;

namespace CodeBase.Services.ConnectionsHandlerService
{
    public class ConnectionsHandler : IService
    {
        public void AddPlayerForConnection(NetworkConnection conn, GameObject player)
        {
            NetworkServer.AddPlayerForConnection(NetworkServer.connections[conn.connectionId], player);
        }
    }
}