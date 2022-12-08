using CodeBase.Infrastructure.ServiceLocator;
using CodeBase.Networking;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.Lobby;
using CodeBase.StaticData.Strings;
using CodeBase.Tools;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Services.Factories
{
    public class UIFactory : IUIFactory
    {
        private readonly AllServices _allServices;
        private readonly IAssetProvider _assetProvider;
        private LobbySelectorUI _lobbySelectorUI;
        private GameObject _uiRoot;


        public UIFactory(AllServices allServices)
        {
            _assetProvider = allServices.Single<IAssetProvider>();
            _allServices = allServices;
        }

        public GameObject CreateUIRoot()
        {
            return _uiRoot = Object.Instantiate(_assetProvider.Load<GameObject>(PrefabsPath.UIRoot));
        }

        public LobbySelectorUI CreateLobbySelector() =>
            _lobbySelectorUI = Object
                .Instantiate(_assetProvider.Load<GameObject>(PrefabsPath.LobbyMenu), _uiRoot.transform)
                .GetComponent<LobbySelectorUI>()
                .With(_ => _.Construct(_allServices.Single<ILobby>(), _allServices.Single<GameNetworkManager>()));

        public EndScreenUI CreateEndScreen(string winnerName) =>
            Object.Instantiate(_assetProvider.Load<GameObject>(PrefabsPath.EndScreenUI))
                .GetComponent<EndScreenUI>()
                .With(_ => _.Construct(winnerName));


    }
}