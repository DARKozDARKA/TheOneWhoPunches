using UnityEngine;

namespace CodeBase.UI
{
    public class CameraLookerUI : MonoBehaviour
    {
        private Transform _cameraTransform;

        private void Awake() => 
            _cameraTransform = Camera.main.transform;

        public void LateUpdate()
        {
            if (_cameraTransform == null)
                return;

            var cameraTransformRotation = _cameraTransform.rotation;
            transform.LookAt(
                transform.position + cameraTransformRotation * Vector3.forward,
                cameraTransformRotation * Vector3.up);
        }
    }
}
