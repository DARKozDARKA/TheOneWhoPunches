using CodeBase.Infrastructure;
using CodeBase.Services.InputHandler;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Characters.Player
{
    public class PlayerClient : NetworkBehaviour
    {
        [SerializeField]
        private PlayerServer _playerServer;
    
        [SerializeField]
        private PlayerMover _mover;

        [SerializeField]
        private PlayerInvulnerability _invulnerability;

        [SerializeField]
        private PlayerSkinChanger _playerSkinChanger;

        [SerializeField]
        private Text _scoreText;
    
        private IInputService _inputService;
    
        private void Awake()
        {
            _mover.OnDashOver += StopDash;
            _invulnerability.OnAttacked += SetAttacked;
            _invulnerability.OnNormalized += SetNormal;
        }

        private void OnDestroy()
        {
            _mover.OnDashOver -= StopDash;
            _invulnerability.OnAttacked -= SetAttacked;
            _invulnerability.OnNormalized -= SetNormal;
        }

        private void Update()
        {
            if (isLocalPlayer == false)
                return;
            
            if (_inputService.GetLMBDown() && _mover.HasSpeed())
                _playerServer.TryStartDashOnServer();
        }
        
        public void ConstructOnClient(GameObject camera)
        {
            _inputService = AllServices.Container.Single<IInputService>();
            _mover.Construct(camera, _inputService);
            GetComponentInChildren<Canvas>().worldCamera = camera.GetComponent<Camera>();
        }

        [ClientRpc]
        public void StartDash() => 
            _mover.StartDash();

        private void StopDash() =>
            _playerServer.StopDash();

        [ClientRpc]
        public void SetScoreText(int score) => 
            _scoreText.text = score.ToString();

        [ClientRpc]
        private void SetAttacked() =>
            _playerSkinChanger.SetDamagedSkin();

        [ClientRpc]
        private void SetNormal() =>
            _playerSkinChanger.SetNormalSkin();
    }
}