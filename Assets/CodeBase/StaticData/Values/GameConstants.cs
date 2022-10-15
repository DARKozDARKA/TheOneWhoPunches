using UnityEngine;

namespace CodeBase.StaticData.Values
{
    public static class GameConstants
    {
        public const float MouseRotateSpeed = 0.8f;

        public const float SlerpValue = 0.25f;

        public const float MinVerticalCameraAngle = 0;
        public const float MaxVerticalCameratAngle = 45;

        public const float Gravity = 1;
        
        public static readonly Vector3 CameraOffset = new(-5, 5, 0);

        public const float PlayerHasSpeedValue = 0.1f;

        public const int WinnerScore = 3;
        public const int RematchTimeInMilliseconds = 5000;
    }
}