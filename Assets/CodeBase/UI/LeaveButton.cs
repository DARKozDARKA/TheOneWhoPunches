using CodeBase.Infrastructure;
using CodeBase.Services.ApplicationRunner;
using UnityEngine;
using UnityEngine.UI;

public class LeaveButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    private IApplicationRunner _applicationRunner;

    private void Awake() =>
        _applicationRunner = AllServices.Container.Single<IApplicationRunner>();

    private void OnEnable() => 
        _button.onClick.AddListener(Leave);

    private void Leave() =>
        _applicationRunner.Quit();

}
