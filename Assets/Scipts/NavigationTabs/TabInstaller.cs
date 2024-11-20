using UnityEngine;
using Zenject;

public class TabInstaller : MonoInstaller
{
    [SerializeField] private TabView tabView;
    [SerializeField] private TabController tabController;

    public override void InstallBindings()
    {
        Container.Bind<TabModel>().AsSingle();
        Container.BindInstance(tabView).AsSingle();
        Container.BindInstance(tabController).AsSingle();
        Container.Bind<Navigation>().FromComponentInHierarchy().AsSingle();
    }
}