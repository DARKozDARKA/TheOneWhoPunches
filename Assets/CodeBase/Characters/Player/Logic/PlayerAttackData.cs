using CodeBase.Characters.Player.Presenter;

namespace CodeBase.Characters.Player.Logic
{
    public class PlayerAttackData
    {
        public PlayerMessenger Attacker;
        public int AttackerPlayerID;
        public PlayerMessenger Victim;

        public PlayerAttackData(PlayerMessenger attacker, int attackerPlayerID, PlayerMessenger victim)
        {
            Attacker = attacker;
            AttackerPlayerID = attackerPlayerID;
            Victim = victim;
        }
    }
}