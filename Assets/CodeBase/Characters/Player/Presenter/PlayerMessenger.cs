using System;
using CodeBase.Characters.Player.Logic;
using CodeBase.Infrastructure.States.GameLoop;
using CodeBase.Messenger.Messages;
using Mirror;

namespace CodeBase.Characters.Player.Presenter
{
    public class PlayerMessenger : NetworkBehaviour
    {
        private PlayerGateaway _playerGateaway;
        
        public Action<int> OnClientScoreChanged;
        public Action OnClientWasAttacked;

        public void Construct(PlayerGateaway gateaway) =>
            _playerGateaway = gateaway;
        

        [Command(requiresAuthority = false)]
        public void SendToServerPlayerAttack(PlayerMessenger enemyPlayerMessenger, int ID)
        {
            _playerGateaway.HandleMessage(new ToServerPlayerAttack(CreateAttackData()));

            PlayerAttackData CreateAttackData() => 
                new PlayerAttackData(attacker: this, attackerPlayerID: ID, victim: enemyPlayerMessenger);
        }


        [ClientRpc]
        public void SendToClientScoreChanged(int score) => 
            OnClientScoreChanged?.Invoke(score);

        [ClientRpc]
        public void SendToClientWasAttacked() => 
            OnClientWasAttacked?.Invoke();
    }
}