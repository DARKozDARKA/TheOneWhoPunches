using CodeBase.Characters.Player.Logic;
using CodeBase.Characters.Player.Presenter;

namespace CodeBase.Infrastructure.States.GameLoop
{
    public class PlayerGateaway
    {
        private readonly PlayersScore _playersScore;

        public PlayerGateaway(PlayersScore playerScore)
        {
            _playersScore = playerScore;
        }

        public void AddPlayer(PlayerMessenger playerMessenger, int ID)
        {
            playerMessenger.OnServerPlayerAttack += SetAttack;
        }

        private void SetAttack(PlayerAttackData data)
        {
            _playersScore.ScorePlayer(data.AttackerPlayerID);
            data.Attacker.SendToClientScoreChanged(_playersScore.GetPlayerScore(data.AttackerPlayerID));
            data.Victim.SendToClientWasAttacked();
        }
    }
}