using CodeBase.Networking;
using CodeBase.Services.Lobby;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class LobbySelectorUI : MonoBehaviour
    {
        [SerializeField]
        private Button _createRoomButton;

        [SerializeField]
        private Button _joinRoomButton;

        [SerializeField]
        private InputField _addressField;
        
        [SerializeField]
        private InputField _nameField;

        public string PlayerName => _nameField.text;

        private GameNetworkManager _gameNetworkManager;

        private ILobby _lobby;

        private void OnEnable()
        {
            _createRoomButton.onClick.AddListener(CreateRoom);
            _joinRoomButton.onClick.AddListener(JoinRoom);
        }

        public void Construct(ILobby lobby, GameNetworkManager gameNetworkManager)
        {
            _gameNetworkManager = gameNetworkManager;
            _lobby = lobby;
        }

        private void CreateRoom()
        {
            _lobby.StartHost();
        }

        private void JoinRoom()
        {
            _lobby.StartClient(_addressField.text);
        }
    }
}