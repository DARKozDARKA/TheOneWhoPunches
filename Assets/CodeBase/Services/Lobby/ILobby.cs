using CodeBase.Infrastructure;

namespace CodeBase.Services.Lobby
{
    public interface ILobby : IService
    {
        void CreateLobbySelector();
        void StartHost();
        void StartClient(string address);
    }
}