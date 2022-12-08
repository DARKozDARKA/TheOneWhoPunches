using CodeBase.Services.Factories;
using Mirror;

namespace CodeBase.Characters.Player.Presenter
{
    public class PlayerIdentity : NetworkBehaviour
    {
    
        public int ID => _id;
        public string PlayerName => _playerName;
    
        [SyncVar]
        private int _id;
        private string _playerName;

        public void LoadData(PlayerServerData data)
        {
            _id = data.ID;
            _playerName = data.Name;
        }
    }
}