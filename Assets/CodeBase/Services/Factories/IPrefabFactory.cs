using CodeBase.Characters.Player;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Services.Factories
{
    public interface IPrefabFactory : IService
    {
        PlayerServer CreatePlayer(Vector3 at);
    }
}