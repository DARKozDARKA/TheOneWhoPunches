using CodeBase.Characters.Player;
using Mirror;

namespace CodeBase.Networking.Messages
{
    public struct SendToClientNewPlayer : NetworkMessage
    {
        public PlayerServer PlayerObject;

        public SendToClientNewPlayer(PlayerServer player)
        {
            PlayerObject = player;
        }
    }
}