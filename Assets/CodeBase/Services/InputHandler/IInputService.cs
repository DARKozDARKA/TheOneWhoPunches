using CodeBase.Infrastructure;

namespace CodeBase.Services.InputHandler
{
    public interface IInputService : IService
    {
        float GetHorizontal();
        float GetVertical();
        float GetMouseY();
        float GetMouseX();
        bool GetLMBDown();
        bool GetESCDown();
    }
}