using CodeBase.Networking;
using CodeBase.Services.ApplicationRunner;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.Factories;
using CodeBase.Services.InputHandler;
using CodeBase.Services.Lobby;
using CodeBase.Services.Mouse;
using CodeBase.Services.StaticData;
using CodeBase.Services.UserData;
using CodeBase.StaticData.Strings;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameNetworkManager _networkManager;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly GameStateMachine _stateMachine;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, GameNetworkManager networkManager,
            AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _networkManager = networkManager;

            RegisterServices();

            services.Single<IStaticDataProvider>().Load();
        }

        public void Enter() =>
            _sceneLoader.Load(SceneNames.Boostrap, EnterLobby);

        public void Exit() { }

        private void EnterLobby() =>
            _stateMachine.Enter<LobbyState>();

        private void RegisterServices()
        {
            RegisterUtilities();
            RegisterFactories();
            RegisterLobby();
        }

        private void RegisterUtilities()
        {
            _services.RegisterSingle(_networkManager);
            _services.RegisterSingle<IUserDataProvider>(new UserDataProvider());
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IInputService>(new InputService());
            _services.RegisterSingle<IStaticDataProvider>(new StaticDataProvider());
            _services.RegisterSingle<IMouseService>(new MouseService());
            _services.RegisterSingle<IApplicationRunner>(new ApplicationRunner());
        }

        private void RegisterFactories()
        {
            _services.RegisterSingle<IUIFactory>(new UIFactory(_services));
            _services.RegisterSingle<IPrefabFactory>(new PrefabFactory(_services));
        }

        private void RegisterLobby() =>
            _services.RegisterSingle<ILobby>(new Lobby(_networkManager, _services.Single<IUIFactory>(),
                _services.Single<IUserDataProvider>()));
    }
}