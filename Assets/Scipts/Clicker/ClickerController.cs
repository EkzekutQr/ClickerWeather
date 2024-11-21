using UniRx;
using Zenject;

public interface IClickerController
{
    void OnClick();
}

public class ClickerController
{
    private readonly CurrencyModel _currencyModel;
    private readonly ClickerView _clickerView;
    private readonly GameSettings _gameSettings;

    public IReadOnlyReactiveProperty<int> Currency => _currencyModel.Currency;
    public IReadOnlyReactiveProperty<int> Energy => _currencyModel.Energy;

    [Inject]
    public ClickerController(CurrencyModel currencyModel, ClickerView clickerView, GameSettings gameSettings)
    {
        _currencyModel = currencyModel;
        _clickerView = clickerView;
        _gameSettings = gameSettings;

        Initialize();
    }

    private void Initialize()
    {
        Observable.Interval(System.TimeSpan.FromSeconds(3))
        .Where(_ => _currencyModel.Energy.Value > 0)
        .Subscribe(_ => OnAutoCollect())
        .AddTo(_clickerView);

        Observable.Interval(System.TimeSpan.FromSeconds(10))
        .Subscribe(_ => _currencyModel.AddEnergy(_gameSettings.energyPerInterval, _gameSettings.maxEnergy))
        .AddTo(_clickerView);
    }

    public void OnClick()
    {
        if (_currencyModel.Energy.Value > 0)
        {
            _currencyModel.AddCurrency(_gameSettings.currencyPerClick);
            _currencyModel.SpendEnergy(_gameSettings.energyCostPerClick);
            _clickerView.PlayClickVFX();
            _clickerView.SpawnCoin();
        }
    }

    private void OnAutoCollect()
    {
        if (_currencyModel.Energy.Value > 0)
        {
            _currencyModel.AddCurrency(_gameSettings.currencyPerClick);
            _currencyModel.SpendEnergy(_gameSettings.energyCostPerAutoCollect);
            _clickerView.PlayClickVFX();
            _clickerView.SpawnCoin();
        }
    }
}