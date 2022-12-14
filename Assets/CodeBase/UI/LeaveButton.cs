using CodeBase.Infrastructure.ServiceLocator;
using CodeBase.Services.ApplicationRunner;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class LeaveButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        private IApplicationRunner _applicationRunner;

        public void Construct(IApplicationRunner applicationRunner) =>
            _applicationRunner = applicationRunner;

        private void OnEnable() => 
            _button.onClick.AddListener(Leave);

        private void Leave() =>
            _applicationRunner.Quit();

    }
}
