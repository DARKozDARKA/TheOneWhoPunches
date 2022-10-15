using CodeBase.StaticData.Strings;
using UnityEngine;

namespace CodeBase.Services.InputHandler
{
    public class InputService : IInputService
    {
        public float GetHorizontal() => 
            Input.GetAxis(InputNames.Horizontal);

        public float GetVertical() => 
            Input.GetAxis(InputNames.Vertical);
        
        public float GetMouseX() => 
            Input.GetAxis(InputNames.MouseX);
        
        public float GetMouseY() => 
            Input.GetAxis(InputNames.MouseY);

        public bool GetLMBDown() =>
            Input.GetKeyDown(KeyCode.Mouse1);
        
        public bool GetESCDown() =>
            Input.GetKeyDown(KeyCode.Escape);
    }
}