using System;
using System.Collections;
using CodeBase.Characters.Player.Logic;
using CodeBase.StaticData.ScriptableObjects;
using Mirror;
using UnityEngine;

namespace CodeBase.Characters.Player.Presenter
{
    public class PlayerInvincibility : NetworkBehaviour, IRequestPlayerData
    {
        public Action OnAttacked;
        public Action OnNormalized;
        
        [SyncVar]
        private bool _isInvincible;
        [SyncVar]
        private float _invicibleTime;

        public bool IsInvincible => _isInvincible;

        public void LoadPlayerData(PlayerData playerData) => 
            _invicibleTime = playerData.PlayerInvincibleTime;

        public void TryAttackThisPlayer()
        {
            if (_isInvincible)
                return;

            StartCoroutine(SetInvincible());
        }

        private IEnumerator SetInvincible()
        {
            _isInvincible = true;
            OnAttacked?.Invoke();
            yield return new WaitForSeconds(_invicibleTime);
            _isInvincible = false;
            OnNormalized?.Invoke();
        }


    }
}