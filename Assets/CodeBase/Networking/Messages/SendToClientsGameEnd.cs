using Mirror;

namespace CodeBase.Networking.Messages
{
    public struct SendToClientsGameEnd : NetworkMessage
    {
        public string WinnerName;
    }
}