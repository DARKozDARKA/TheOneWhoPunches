using CodeBase.Infrastructure.ServiceLocator;
using UnityEngine;

namespace CodeBase.Services.Factories
{
    public interface IPrefabFactory : IService
    {
        GameObject CreatePlayer(PlayerServerData data, Vector3 at);
    }
}