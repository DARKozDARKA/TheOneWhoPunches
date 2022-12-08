using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Characters.Player.Vizualization
{
    public class PlayerScore : NetworkBehaviour 
    {
        [SerializeField]
        private Text _scoreText;
        
        public void SetScoreText(int score) => 
            _scoreText.text = score.ToString();
    }
}