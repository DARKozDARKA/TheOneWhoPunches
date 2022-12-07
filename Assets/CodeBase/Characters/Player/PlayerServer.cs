using System;
using CodeBase.StaticData.ScriptableObjects;
using Mirror;
using UnityEngine;

namespace CodeBase.Characters.Player
{
    public class PlayerServer : NetworkBehaviour
    {
        [SerializeField]
        private PlayerClient _playerClient;
    
        [SerializeField]
        private TriggerCollideServerDetector _triggerCollideServerDetector;
    
        [SerializeField]
        private PlayerInvulnerability _invulnerability;
    
        [SerializeField]
        private PlayerMover _mover;
        public int ID => _id;
        public string PlayerName => _playerName;
    
        private int _id;
        private int _score;
        private bool _isDashing;
        private string _playerName;

        public Action<PlayerServer, int> OnScoreChanged;

        private void Awake() => 
            _triggerCollideServerDetector.OnCollisionEntered += HandleCollision;

        private void OnDestroy() => 
            _triggerCollideServerDetector.OnCollisionEntered -= HandleCollision;

        public void LoadDataFromServer(int id, string playerName)
        {
            _playerName = playerName;
            _id = id;
        }
    
        public bool TryAttackThisPlayer() =>
            _invulnerability.TryAttackThisPlayer();
    
        [Command]
        public void TryStartDashOnServer()
        {
            if (_isDashing)
                return;
        
            _playerClient.StartDash();
            _isDashing = true;
        }
    
        [Command]
        public void StopDash() => 
            _isDashing = false;

        private void HandleCollision(GameObject collideObject)
        {
            if (_isDashing == false)
                return;
            
            if (collideObject.TryGetComponent<PlayerServer>(out var collidedPlayer) == false)
                return;

            if (collidedPlayer.ID == ID)
                return;

            if (collidedPlayer.TryAttackThisPlayer())
                AddScore();
            
        }

        private void AddScore()
        {
            _score++;
            OnScoreChanged?.Invoke(this, _score);
            SetScoreOnClient();
        }

        private void SetScoreOnClient() =>
            _playerClient.SetScoreText(_score);
    }
}
