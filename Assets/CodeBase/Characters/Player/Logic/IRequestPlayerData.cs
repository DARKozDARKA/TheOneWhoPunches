using CodeBase.StaticData.ScriptableObjects;

namespace CodeBase.Characters.Player.Logic
{
    public interface IRequestPlayerData
    {
        void LoadPlayerData(PlayerData playerData);
    }
}