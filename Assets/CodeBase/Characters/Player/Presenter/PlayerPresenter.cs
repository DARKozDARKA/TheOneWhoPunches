using System;
using UnityEngine;

namespace CodeBase.Characters.Player.Presenter
{
    public class PlayerPresenter : MonoBehaviour
    {
        [SerializeField]
        private PlayerMessenger _playerMessenger;

        [SerializeField]
        private PlayerIdentity _playerIdentity;

        [SerializeField]
        private PlayerInvincibility _playerInvincibility;

        public Action OnAttacked;
        public Action OnNormalized;

        private void OnEnable()
        {
            _playerMessenger.OnClientWasAttacked += OnPlayerAttacked;
            _playerInvincibility.OnNormalized += OnPlayerNormalized;
        }
        
        private void OnDisable()
        {
            _playerMessenger.OnClientWasAttacked -= OnPlayerAttacked;
            _playerInvincibility.OnNormalized -= OnPlayerNormalized;
        }

        public PlayerMessenger GetMessenger() =>
            _playerMessenger;
        
        public bool CanBeAttacked() => 
            !_playerInvincibility.IsInvincible;

        public int GetPlayerID() =>
            _playerIdentity.ID;

        private void OnPlayerAttacked()
        {
            OnAttacked?.Invoke();
            _playerInvincibility.TryAttackThisPlayer();
        }
        

        private void OnPlayerNormalized() => 
            OnNormalized?.Invoke();
    }
}