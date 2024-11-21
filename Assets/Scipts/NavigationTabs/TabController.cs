using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class TabController : MonoBehaviour
{
    private TabModel _model;
    private TabView _view;
    private WeatherController _weatherController;
    private FactsController _factsController;
    public bool IsSwitchingTabs { get; private set; }

    [Inject]
    public void Construct(TabModel model, TabView view, WeatherController weatherController, FactsController factsController)
    {
        _model = model;
        _view = view;
        _weatherController = weatherController;
        _factsController = factsController;
    }

    private void Start()
    {
        _view.OnClickerButtonClicked()
        .Subscribe(_ => SwitchTab("Clicker").Forget())
        .AddTo(this);

        _view.OnFactsButtonClicked()
        .Subscribe(_ => SwitchTab("Facts").Forget())
        .AddTo(this);
    }

    public async UniTask SwitchTab(string tabName)
    {
        if (_model.ActiveTab.Value == tabName || IsSwitchingTabs)
            return;

        IsSwitchingTabs = true;
        _view.SetButtonsInteractable(false);
        await _view.PlayFadeOutAnimation();
        _model.SetActiveTab(tabName);
        await _view.PlayFadeInAnimation();
        _view.SetButtonsInteractable(true);
        IsSwitchingTabs = false;

        if (tabName == "Clicker")
        {
            _weatherController.SetClickerScreenActive(true);
            _factsController.OnFactsTabSelected();
        }
        else
        {
            _weatherController.SetClickerScreenActive(false);
            _factsController.OnFactsTabSelected();
        }
    }
}