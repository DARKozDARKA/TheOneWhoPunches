using CodeBase.Infrastructure;
using CodeBase.Services.InputHandler;
using CodeBase.StaticData.Values;
using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.CameraScripts
{
    public class CameraFollower : MonoBehaviour
    {
        private IInputService _inputService;
        
        private bool _isTargetSet;
        
        private GameObject _target;
        
        private float _rotationX;
        private float _rotationY;
        private Vector2 _swipeDirection;
        private Quaternion _cameraRotation;
        private float _distanceBetweenCameraAndTarget;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
        }

        private void Update()
        {
            if (_isTargetSet == false)
                return;

            _rotationX += _inputService.GetMouseX() * GameConstants.MouseRotateSpeed;
            _rotationY += -_inputService.GetMouseY() * GameConstants.MouseRotateSpeed;

            MathTool.LimitValueInBoundries(
                value: ref _rotationY,
                minValue: GameConstants.MinVerticalCameraAngle,
                maxValue: GameConstants.MaxVerticalCameratAngle);
        }

        private void LateUpdate()
        {
            if (_target == null)
                return;

            var direction = new Vector3(0, 0, -_distanceBetweenCameraAndTarget);
            Quaternion rotation = Quaternion.Euler(_rotationY, _rotationX, 0);

            _cameraRotation = Quaternion.Slerp(_cameraRotation, rotation, GameConstants.SlerpValue);

            Vector3 targetPosition = _target.transform.position;

            transform.position = targetPosition + _cameraRotation * direction;
            transform.LookAt(targetPosition);
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
            _isTargetSet = true;

            Vector3 position = target.transform.position;
            transform.position = position + GameConstants.CameraOffset;

            _distanceBetweenCameraAndTarget = Vector3.Distance(transform.position, position);
        }
    }
}