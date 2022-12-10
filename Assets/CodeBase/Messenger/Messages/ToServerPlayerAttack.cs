using CodeBase.Characters.Player.Logic;

namespace CodeBase.Messenger.Messages
{
    public class ToServerPlayerAttack : ToServerMessage
    {
        public PlayerAttackData AttackData;

        public ToServerPlayerAttack(PlayerAttackData data) =>
            AttackData = data;
    }
}