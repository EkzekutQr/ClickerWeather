using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private ClickerView clickerView;
    [SerializeField] private GameSettings gameSettings;

    public override void InstallBindings()
    {
        Container.Bind<CurrencyModel>().AsSingle();
        Container.Bind<ClickerController>().AsSingle().WithArguments(clickerView, gameSettings);
        Container.BindInstance(gameSettings).AsSingle();
    }
}