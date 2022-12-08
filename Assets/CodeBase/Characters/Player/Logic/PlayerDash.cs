using System;
using CodeBase.Infrastructure.ServiceLocator;
using CodeBase.Services.InputHandler;
using CodeBase.StaticData.ScriptableObjects;
using Mirror;
using UnityEngine;

namespace CodeBase.Characters.Player.Logic
{
    public class PlayerDash : NetworkBehaviour, IRequestPlayerData
    {
        [SerializeField]
        private PlayerMover _playerMover;
    
        private IInputService _inputService;

        [SyncVar]
        private float _dashDistance;

        [SyncVar]
        private float _dashSpeedModifier;
    
        [SyncVar]
        private bool _isDashing;
        private Vector3 _dashingStartPosition;

        public Action OnDashEnd;

        public bool IsDashing => _isDashing;

        private void Awake()
        {
            Construct(AllServices.Container.Single<IInputService>());
        }

        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
    
        private void Update()
        {
            if (_inputService.GetLMBDown() && CanStartDash() && _playerMover.HasSpeed())
                StartDash();

            if (_isDashing && IfDashIsOver())
                StopDash();
        }

        public void LoadPlayerData(PlayerData playerData)
        {
            _dashDistance = playerData.DashDistance;
            _dashSpeedModifier = playerData.DashSpeedModifier;
        }

        private void StartDash()
        {
            _isDashing = true;
            _dashingStartPosition = transform.position;

            EnableDashOnMover();
        }

        private void StopDash()
        {
            DisableDashOnMover();
            _isDashing = false;
            OnDashEnd?.Invoke();
        }

        private void EnableDashOnMover()
        {
            _playerMover.SetSpeedMultiplier(_dashSpeedModifier);
            _playerMover.DisableControl();
            _playerMover.DisableGravity();
        }

        private void DisableDashOnMover()
        {
            _playerMover.NormalizeSpeedMultiplier();
            _playerMover.EnableControl();
            _playerMover.EnableGravity();
        }

        private bool IfDashIsOver() =>
            Vector3.Distance(transform.position, _dashingStartPosition) >= _dashDistance;

        private bool CanStartDash() =>
            !_isDashing;
    }
}