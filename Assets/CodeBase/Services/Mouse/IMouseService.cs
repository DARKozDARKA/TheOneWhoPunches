using CodeBase.Infrastructure;

namespace CodeBase.Services.Mouse
{
    public interface IMouseService : IService
    {
        void LockMouse();
        void ConfineMouse();
    }
}