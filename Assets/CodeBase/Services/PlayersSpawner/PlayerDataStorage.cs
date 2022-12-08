using System.Collections.Generic;
using CodeBase.Services.Factories;

namespace CodeBase.Services.PlayersSpawner
{
    public class PlayerDataStorage
    {
        private Dictionary<int, PlayerServerData> _playersServerData = new();

        public void AddPlayerData(int ID, PlayerServerData data) =>
            _playersServerData.Add(ID, data);
    
        public void RemovePlayerData(int ID) =>
            _playersServerData.Remove(ID);

        public PlayerServerData GetData(int ID) =>
            _playersServerData[ID];
    }
}