using UnityEngine;

namespace CodeBase.Tools
{
    public static class MathTool
    {
        public static Vector3 AdjustDirectionDependingOnObjectAngel(Vector3 direction, GameObject targetObject)
        {
            var angleAxis = Quaternion.AngleAxis(targetObject.transform.rotation.eulerAngles.y, Vector3.up);

            var forward = angleAxis * Vector3.forward;
            var right = angleAxis * Vector3.right;

            var moveDirection = forward * direction.y + right * direction.x;

            return moveDirection.normalized;
        }

        public static void LimitValueInBoundries(ref float value, float minValue, float maxValue)
        {
            if (value < minValue)
                value = minValue;
            else if (value > maxValue)
                value = maxValue;
        }
    }
}