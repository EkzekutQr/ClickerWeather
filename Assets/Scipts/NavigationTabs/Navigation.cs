using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;
using Cysharp.Threading.Tasks;

public class Navigation : MonoBehaviour
{
    [SerializeField] private Button clickerButton;
    [SerializeField] private Button factsButton;

    private TabController _tabController;

    [Inject]
    public void Construct(TabController tabController)
    {
        _tabController = tabController;
    }

    void Start()
    {
        clickerButton.OnClickAsObservable()
        .Subscribe(_ => SwitchTab("Clicker"))
        .AddTo(this);

        factsButton.OnClickAsObservable()
        .Subscribe(_ => SwitchTab("Facts"))
        .AddTo(this);
    }

    private void SwitchTab(string tabName)
    {
        if (_tabController.IsSwitchingTabs)
            return;

        _tabController.SwitchTab(tabName).Forget();
    }
}