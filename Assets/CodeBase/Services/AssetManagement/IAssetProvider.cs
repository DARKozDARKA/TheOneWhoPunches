using CodeBase.Infrastructure.ServiceLocator;
using UnityEngine;

namespace CodeBase.Services.AssetManagement
{
    public interface IAssetProvider : IService
    {
        T Load<T>(string path) where T : Object;
        T[] LoadAll<T>(string path) where T : Object;
    }
}