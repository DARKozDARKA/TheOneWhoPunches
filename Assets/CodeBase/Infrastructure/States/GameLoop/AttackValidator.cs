using CodeBase.Characters.Player.Logic;
using CodeBase.Messenger;
using CodeBase.Messenger.Messages;

namespace CodeBase.Infrastructure.States.GameLoop
{
    public class AttackValidator
    {
        private readonly PlayersScore _playersScore;
        
        public AttackValidator(PlayerGateaway playerGateaway, PlayersScore playerScore)
        {
            _playersScore = playerScore;
            playerGateaway.RegisterToMessage<ToServerPlayerAttack>(SetAttack);
        }
        
        private void SetAttack(ToServerMessage message)
        {
            PlayerAttackData attackData = ((ToServerPlayerAttack)message).AttackData;
            _playersScore.ScorePlayer(attackData.AttackerPlayerID);
            attackData.Attacker.SendToClientScoreChanged(_playersScore.GetPlayerScore(attackData.AttackerPlayerID));
            attackData.Victim.SendToClientWasAttacked();
        }
    }
}