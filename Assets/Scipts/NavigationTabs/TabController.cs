using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class TabController : MonoBehaviour
{
    private TabModel _model;
    private TabView _view;
    public bool IsSwitchingTabs { get; private set; }

    [Inject]
    public void Construct(TabModel model, TabView view)
    {
        _model = model;
        _view = view;
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
    }
}