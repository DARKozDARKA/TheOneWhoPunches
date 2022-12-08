using CodeBase.Characters.Player;
using Mirror;
using UnityEngine;

namespace CodeBase.Networking.Messages
{
    public struct SendToClientNewPlayer : NetworkMessage
    {
        public GameObject PlayerObject;

        public SendToClientNewPlayer(GameObject player)
        {
            PlayerObject = player;
        }
    }
}