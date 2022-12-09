using CodeBase.Services.InputHandler;
using CodeBase.StaticData.ScriptableObjects;
using CodeBase.StaticData.Values;
using CodeBase.Tools;
using Mirror;
using UnityEngine;

namespace CodeBase.Characters.Player.Logic
{
    public class PlayerMover : NetworkBehaviour, IRequestPlayerData
    {
        [SerializeField]
        private CharacterController _characterController;

        private IInputService _inputService;

        [SyncVar]
        private float _speed;

        private GameObject _camera;
        private Vector3 _direction;
        private float _verticalSpeed;

        private bool _gravityIsEnabled = true;
        private bool _isDirectrlyControlled = true;

        private float _speedMultiplier = 1;
        
        public void Construct(GameObject camera, IInputService inputService)
        {
            _inputService = inputService;
            _camera = camera;
        }

        private void Update()
        {
            if (isLocalPlayer == false)
                return;

            if (_isDirectrlyControlled)
                CalculateMovementDirection();
            
            if (_gravityIsEnabled)
                ApplyGravity();

            MoveCharacter();
        }

        public void LoadPlayerData(PlayerData playerData) => 
            _speed = playerData.Speed;

        public bool HasSpeed() =>
            _direction.magnitude >= GameConstants.PlayerHasSpeedValue;

        public void SetSpeedMultiplier(float multiplier) => 
            _speedMultiplier = multiplier;

        public void NormalizeSpeedMultiplier() =>
            SetSpeedMultiplier(1);

        public void DisableGravity() => 
            _gravityIsEnabled = false;

        public void EnableGravity() => 
            _gravityIsEnabled = true;

        public void DisableControl() => 
            _isDirectrlyControlled = false;

        public void EnableControl() => 
            _isDirectrlyControlled = true;

        private void MoveCharacter() => 
            _characterController.Move(_direction * (Time.deltaTime * _speed * _speedMultiplier));

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
    }
}