using System.Linq;
using CodeBase.Characters.Player;
using CodeBase.Infrastructure;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.ScriptableObjects;
using CodeBase.StaticData.Strings;
using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.Services.Factories
{
    public class PrefabFactory : IPrefabFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataProvider _staticDataProvider;

        public PrefabFactory(AllServices allServices)
        {
            _assetProvider = allServices.Single<IAssetProvider>();
            _staticDataProvider = allServices.Single<IStaticDataProvider>();
        }

        public PlayerServer CreatePlayer(Vector3 at)
        {
            return Object.Instantiate(_assetProvider.Load<GameObject>(PrefabsPath.Player), at, Quaternion.identity)
                .With(LoadPlayerDataIntoGameObject)
                .GetComponent<PlayerServer>();
        }

        private void LoadPlayerDataIntoGameObject(GameObject gameObject)
        {
            PlayerData playerData = _staticDataProvider.GetPlayerData();
            gameObject.GetComponentsInChildren<IRequestPlayerData>()
                .ToList()
                .ForEach(component => component.LoadPlayerData(playerData));
        }

    }
}