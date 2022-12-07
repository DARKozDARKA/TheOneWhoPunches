using CodeBase.StaticData.ScriptableObjects;

namespace CodeBase.Characters.Player
{
    public interface IRequestPlayerData
    {
        void LoadPlayerData(PlayerData playerData);
    }
}