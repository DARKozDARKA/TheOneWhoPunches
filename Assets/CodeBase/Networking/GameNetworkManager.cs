using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.ServiceLocator;
using Mirror;

namespace CodeBase.Networking
{
    public class GameNetworkManager : NetworkManager, IService
    {
        public Action OnClientConnected;
        public Action OnClientStop;

        public Action<NetworkConnectionToClient> OnServerDisconnected;
        public List<NetworkConnectionToClient> ConnectionToClients { get; } = new();

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            ConnectionToClients.Add(conn);
        }
        
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            OnServerDisconnected?.Invoke(conn);
            ConnectionToClients.Remove(conn);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            OnClientConnected?.Invoke();
        }
        
        public override void OnStopClient() => 
            OnClientStop?.Invoke();
    }
}