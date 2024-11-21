using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private ClickerView clickerView;
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private WeatherView weatherView;
    [SerializeField] private FactsView factsView;

    public override void InstallBindings()
    {
        Container.Bind<CurrencyModel>().AsSingle();
        Container.Bind<ClickerController>().AsSingle().WithArguments(clickerView, gameSettings);
        Container.BindInstance(gameSettings).AsSingle();
        Container.Bind<RequestQueue>().AsSingle();
        Container.Bind<WeatherModel>().AsSingle();
        Container.Bind<WeatherController>().AsSingle();
        Container.Bind<FactsModel>().AsSingle();
        Container.Bind<FactsController>().AsSingle().WithArguments(factsView);
    }
}