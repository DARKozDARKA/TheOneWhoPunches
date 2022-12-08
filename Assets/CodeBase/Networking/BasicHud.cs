using CodeBase.Services.InputHandler;
using Mirror;
using UnityEngine;

namespace CodeBase.Networking
{
    public class BasicHud : MonoBehaviour
    {
        private const int BaseOffsetX = 10;
        private const int BaseOffsetY = 40;
        private const int Width = 215;
        private const int Height = 9999;

        [SerializeField]
        private int OffsetX;

        [SerializeField]
        private int OffsetY;

        private NetworkManager _manager;
        private IInputService _inputService;

        public void Construct(GameNetworkManager gameNetworkManager, IInputService inputService)
        {
            _manager = gameNetworkManager;
            _inputService = inputService;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(BaseOffsetX + OffsetX, BaseOffsetY + OffsetY, Width, Height));
            if (!NetworkClient.isConnected && !NetworkServer.active)
                StartButtons();
            else
                StatusLabels();

            if (NetworkClient.isConnected && !NetworkClient.ready)
                if (GUILayout.Button("Client Ready"))
                {
                    NetworkClient.Ready();
                    if (NetworkClient.localPlayer == null)
                        NetworkClient.AddPlayer();
                }

            StopButtons();

            GUILayout.EndArea();
        }

        private void StartButtons()
        {
            if (!NetworkClient.active)
            {
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                    if (GUILayout.Button("Host (Server + Client)"))
                        _manager.StartHost();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Client"))
                    _manager.StartClient();

                _manager.networkAddress = GUILayout.TextField(_manager.networkAddress);
                GUILayout.EndHorizontal();

                if (Application.platform == RuntimePlatform.WebGLPlayer)
                    GUILayout.Box("(  WebGL cannot be server  )");
            }
            else
            {
                GUILayout.Label($"Connecting to {_manager.networkAddress}..");
                if (GUILayout.Button("Cancel Connection Attempt"))
                    _manager.StopClient();
            }
        }

        private void StatusLabels()
        {
            if (NetworkServer.active && NetworkClient.active)
                GUILayout.Label($"<b>Host</b>: running via {Transport.activeTransport}");

            else if (NetworkClient.isConnected)
                GUILayout.Label(
                    $"<b>Client</b>: connected to {_manager.networkAddress} via {Transport.activeTransport}");
        }

        private void StopButtons()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Host (press ESC)") || _inputService.GetESCDown())
                    _manager.StopHost();
            }
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Client (press ESC)") || _inputService.GetESCDown())
                    _manager.StopClient();
            }
        }
    }
}