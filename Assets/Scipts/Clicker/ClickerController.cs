using UnityEngine;
using Zenject;

public interface IClickerController
{
    void OnClick();
}

public class ClickerController : IClickerController
{
    private readonly CurrencyManager _currencyManager;
    private readonly ClickerButton _clickerButton;

    [Inject]
    public ClickerController(CurrencyManager currencyManager, ClickerButton clickerButton)
    {
        _currencyManager = currencyManager;
        _clickerButton = clickerButton;
    }

    public void OnClick()
    {
        if (_currencyManager.Energy.Value > 0)
        {
            _currencyManager.OnButtonClick();
            _clickerButton.PlayClickVFX();
            _clickerButton.SpawnCoin();
        }
    }
}