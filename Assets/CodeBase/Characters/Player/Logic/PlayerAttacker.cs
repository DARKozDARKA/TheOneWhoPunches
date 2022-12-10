using CodeBase.Characters.Player.Presenter;
using CodeBase.Messenger;
using CodeBase.Messenger.Messages;
using Mirror;
using UnityEngine;

namespace CodeBase.Characters.Player.Logic
{
    public class PlayerAttacker : NetworkBehaviour
    {
        [SerializeField]
        private PlayerPresenter _playerPresenter;
    
        [SerializeField]
        private PlayerMessenger _playerMessenger;

        [SerializeField]
        private PlayerDash _playerDash;

        [SerializeField]
        private TriggerCollideLocalPlayerDetector _collideDetector;

        private bool _hittedOnAttack;

        private void OnEnable()
        {
            _collideDetector.OnCollisionEntered += TryAttack;
            _playerDash.OnDashEnd += StopAttack;
        }

        private void OnDisable()
        {
            _collideDetector.OnCollisionEntered -= TryAttack;
        }

        private void TryAttack(GameObject collisionObject)
        {
            if (isLocalPlayer == false)
                return;
        
            if (ThisPlayerCanAttack() == false)
                return;
        
            PlayerPresenter otherPlayerPresenter = collisionObject.GetComponent<PlayerPresenter>();

            if (CantAttack(otherPlayerPresenter)) 
                return;

            Attack(otherPlayerPresenter.GetMessenger());
        }

        private bool CantAttack(PlayerPresenter otherPlayerPresenter)
        {
            if (otherPlayerPresenter == null)
                return true;
            if (OtherCanBeAttacked())
                return true;
            if (ThisIsSamePlayer())
                return true;

            return false;
        
            bool OtherCanBeAttacked() => 
                otherPlayerPresenter.CanBeAttacked() == false;
        
            bool ThisIsSamePlayer() =>
                otherPlayerPresenter.GetPlayerID() == _playerPresenter.GetPlayerID();
        }

        private void Attack(PlayerMessenger otherMessenger)
        {
            _hittedOnAttack = true;
            _playerMessenger.SendToServerPlayerAttack(otherMessenger, _playerPresenter.GetPlayerID());
        }

        private void StopAttack()
        {
            _hittedOnAttack = false;
        }

        private bool ThisPlayerCanAttack() => 
            _playerDash.IsDashing && _hittedOnAttack == false;


    }
}