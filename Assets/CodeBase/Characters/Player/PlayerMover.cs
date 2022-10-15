using System;
using CodeBase.Services.InputHandler;
using CodeBase.StaticData.Values;
using CodeBase.Tools;
using Mirror;
using UnityEngine;

namespace CodeBase.Characters.Player
{
    public class PlayerMover : NetworkBehaviour
    {
        [SerializeField]
        private CharacterController _characterController;

        private IInputService _inputService;

        [SyncVar]
        private float _speed;

        [SyncVar]
        private float _dashDistance;

        [SyncVar]
        private float _dashSpeedModifier;

        public Action OnDashOver;

        private GameObject _camera;
        private Vector3 _direction;
        private float _verticalSpeed;

        private bool _isDashing;
        private Vector3 _dashingStartPosition;
        private float DashSpeedMultiplier => _isDashing ? _dashSpeedModifier : 1;

        public void Construct(GameObject camera, IInputService inputService)
        {
            _inputService = inputService;
            _camera = camera;
        }

        public void LoadData(float speed, float dashDistance, float dashSpeedModifier)
        {
            _speed = speed;
            _dashDistance = dashDistance;
            _dashSpeedModifier = dashSpeedModifier;
        }


        private void Update()
        {
            if (isLocalPlayer == false)
                return;

            if (_isDashing == false)
            {
                CalculateMovementDirection();
                ApplyGravity();
            }

            if (_isDashing)
            {
                if (CheckIfDashIsOver())
                    StopDash();
            }

            _characterController.Move(_direction * (DashSpeedMultiplier * Time.deltaTime * _speed));
        }

        public void StartDash()
        {
            _isDashing = true;
            _dashingStartPosition = transform.position;
        }

        public bool CanStartDash() =>
            !_isDashing;

        public bool HasSpeed() =>
            _direction.magnitude >= GameConstants.PlayerHasSpeedValue;

        private void CalculateMovementDirection()
        {
            var direction = new Vector2(_inputService.GetHorizontal(), _inputService.GetVertical());
            _direction = MathTool.AdjustDirectionDependingOnObjectAngel(direction, _camera);
        }

        private void ApplyGravity()
        {
            if (_characterController.isGrounded)
            {
                _verticalSpeed = 0;
                return;
            }

            _verticalSpeed -= GameConstants.Gravity * Time.deltaTime;
            _direction.y = _verticalSpeed;
        }

        private bool CheckIfDashIsOver() =>
            Vector3.Distance(transform.position, _dashingStartPosition) >= _dashDistance;

        private void StopDash()
        {
            _isDashing = false;
            OnDashOver?.Invoke();
        }
    }
}