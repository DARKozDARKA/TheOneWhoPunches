using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData.ScriptableObjects;
using CodeBase.StaticData.Strings;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    public class StaticDataProvider : IStaticDataProvider
    {
        private Dictionary<string, LevelData> _levelsData;
        private PlayerData _playerData;

        public void Load()
        {
            _playerData = LoadResource<PlayerData>(StaticDataPath.PlayerData);

            _levelsData = LoadResources<LevelData>(StaticDataPath.LevelsData)
                .ToDictionary(_ => _.Key, _ => _);
        }

        public PlayerData GetPlayerData()
        {
            return _playerData;
        }

        public LevelData GetLevelData(string key)
        {
            return _levelsData[key];
        }

        private T LoadResource<T>(string path) where T : ScriptableObject
        {
            return Resources.Load<T>(path);
        }

        private T[] LoadResources<T>(string path) where T : ScriptableObject
        {
            return Resources.LoadAll<T>(path);
        }
    }
}