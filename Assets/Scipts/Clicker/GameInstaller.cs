using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private ClickerButton clickerButton;
    [SerializeField] private CurrencyManager currencyManager;

    public override void InstallBindings()
    {
        Container.Bind<CurrencyManager>().FromInstance(currencyManager).AsSingle();
        Container.Bind<ClickerButton>().FromInstance(clickerButton).AsSingle();
        Container.Bind<IClickerController>().To<ClickerController>().AsSingle();
    }
}