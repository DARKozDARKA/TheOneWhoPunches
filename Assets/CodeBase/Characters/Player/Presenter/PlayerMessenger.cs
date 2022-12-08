using System;
using CodeBase.Characters.Player.Logic;
using Mirror;

namespace CodeBase.Characters.Player.Presenter
{
    public class PlayerMessenger : NetworkBehaviour
    {
        public Action<PlayerAttackData> OnServerPlayerAttack;
        public Action<int> OnClientScoreChanged;
        public Action OnClientWasAttacked;

        [Command(requiresAuthority = false)]
        public void SendToServerPlayerAttack(PlayerMessenger enemyPlayerMessenger, int ID)
        {
            OnServerPlayerAttack?.Invoke(new PlayerAttackData()
            {
                Attacker = this,
                AttackerPlayerID = ID,
                Victim = enemyPlayerMessenger
            });
        }

    
        [ClientRpc]
        public void SendToClientScoreChanged(int score) => 
            OnClientScoreChanged?.Invoke(score);

        [ClientRpc]
        public void SendToClientWasAttacked() => 
            OnClientWasAttacked?.Invoke();
    }
}