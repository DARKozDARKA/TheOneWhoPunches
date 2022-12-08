using CodeBase.Infrastructure.States.Bootstrap;
using CodeBase.Networking;
using CodeBase.Services.CoroutineRunner;
using UnityEngine;

namespace CodeBase.Infrastructure.GameRunner
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