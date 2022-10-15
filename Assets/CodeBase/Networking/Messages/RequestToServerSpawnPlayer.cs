using Mirror;

namespace CodeBase.Networking.Messages
{
    public struct RequestToServerSpawnPlayer : NetworkMessage
    {
        public string PlayerName;
    }
}