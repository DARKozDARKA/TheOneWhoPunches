using CodeBase.Infrastructure.ServiceLocator;
using CodeBase.Networking;
using CodeBase.Services.CoroutineRunner;
using CodeBase.Services.SceneLoader;

namespace CodeBase.Infrastructure.GameRunner
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, GameNetworkManager networkManager) => 
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), AllServices.Container, networkManager);
    }
}