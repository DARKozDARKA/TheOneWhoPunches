using CodeBase.Networking;
using CodeBase.Services.Factories;
using CodeBase.Services.UserData;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Services.Lobby
{
    public class Lobby : ILobby
    {
        private readonly GameNetworkManager _gameNetworkManager;
        private readonly IUIFactory _uiFactory;
        private readonly IUserDataProvider _userDataProvider;
        private LobbySelectorUI _lobbySelector;

        public Lobby(GameNetworkManager gameNetworkManager, IUIFactory uiFactory, IUserDataProvider userDataProvider)
        {
            _gameNetworkManager = gameNetworkManager;
            _uiFactory = uiFactory;
            _userDataProvider = userDataProvider;
        }

        public void CreateLobbySelector()
        {
            if (_lobbySelector != null)
                Object.Destroy(_lobbySelector);
            
            _lobbySelector = _uiFactory.CreateLobbySelector();
        }

        public void StartHost()
        {
            SetUserName();
            _gameNetworkManager.StartHost();
        }

        public void StartClient(string address)
        {
            SetUserName();
            _gameNetworkManager.networkAddress = address;
            _gameNetworkManager.StartClient();
        }

        private void SetUserName()
        {
            _userDataProvider.UserData.PlayerName = _lobbySelector.PlayerName;
        }
           

    }
}