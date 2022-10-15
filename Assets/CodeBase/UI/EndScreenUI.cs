using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class EndScreenUI : MonoBehaviour
    {
        [SerializeField]
        private Text _winnerText;

        public void Construct(string winnerName) =>
            _winnerText.text = winnerName;
    }
}
