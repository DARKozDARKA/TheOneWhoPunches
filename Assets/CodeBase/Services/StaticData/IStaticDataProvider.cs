using CodeBase.Infrastructure.ServiceLocator;
using CodeBase.StaticData.ScriptableObjects;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataProvider : IService
    {
        void Load();
        PlayerData GetPlayerData();
        LevelData GetLevelData(string key);
    }
}