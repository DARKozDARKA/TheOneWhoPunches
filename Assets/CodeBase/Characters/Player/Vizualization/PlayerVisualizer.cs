using CodeBase.Characters.Player.Presenter;
using UnityEngine;

namespace CodeBase.Characters.Player.Vizualization
{
    public class PlayerVisualizer : MonoBehaviour
    {
        [SerializeField]
        private PlayerMessenger _messenger;
        
        [SerializeField]
        private PlayerPresenter _presenter;
        
        [SerializeField]
        private PlayerSkinChanger _skinChanger;

        [SerializeField]
        private PlayerScore _playerScore;

        private void OnEnable()
        {
            _presenter.OnAttacked += _skinChanger.SetDamagedSkin;
            _presenter.OnNormalized += _skinChanger.SetNormalSkin;
            _messenger.OnClientScoreChanged += _playerScore.SetScoreText;

        }
        
        private void OnDisable()
        {
            _presenter.OnAttacked -= _skinChanger.SetDamagedSkin;
            _presenter.OnNormalized -= _skinChanger.SetNormalSkin;
            _messenger.OnClientScoreChanged -= _playerScore.SetScoreText;
        }
    }
}