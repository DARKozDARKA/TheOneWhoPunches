using CodeBase.Infrastructure.States;
using CodeBase.Networking;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, GameNetworkManager networkManager) => 
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), AllServices.Container, networkManager);
    }
}