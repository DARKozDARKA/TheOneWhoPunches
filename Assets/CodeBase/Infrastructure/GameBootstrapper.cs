using CodeBase.Infrastructure.States;
using CodeBase.Networking;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField]
        private GameNetworkManager _networkManager;

        private Game _game;

        private void Awake()
        {
            _game = new Game(this, _networkManager);
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}