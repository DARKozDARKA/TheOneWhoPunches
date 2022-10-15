using CodeBase.Characters.Player;
using CodeBase.Infrastructure;
using CodeBase.Services.AssetManagement;
using CodeBase.StaticData.Strings;
using UnityEngine;

namespace CodeBase.Services.Factories
{
    public class PrefabFactory : IPrefabFactory
    {
        private readonly AllServices _allServices;
        private readonly IAssetProvider _assetProvider;

        public PrefabFactory(AllServices allServices)
        {
            _assetProvider = allServices.Single<IAssetProvider>();
            _allServices = allServices;
        }

        public PlayerServer CreatePlayer(Vector3 at)
        {
            return Object.Instantiate(_assetProvider.Load<GameObject>(PrefabsPath.Player), at, Quaternion.identity)
                .GetComponent<PlayerServer>();
        }
    }
}