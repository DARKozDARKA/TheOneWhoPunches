using CodeBase.Infrastructure;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Services.Factories
{
    public interface IUIFactory : IService
    {
        GameObject CreateUIRoot();
        LobbySelectorUI CreateLobbySelector();
        EndScreenUI CreateEndScreen(string winnerName);
    }
}