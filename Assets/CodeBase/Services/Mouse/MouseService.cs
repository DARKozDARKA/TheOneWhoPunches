using UnityEngine;

namespace CodeBase.Services.Mouse
{
    public class MouseService : IMouseService
    {
        public void LockMouse() =>
            Cursor.lockState = CursorLockMode.Locked;
        
        public void ConfineMouse() =>
            Cursor.lockState = CursorLockMode.Confined;
    }
}