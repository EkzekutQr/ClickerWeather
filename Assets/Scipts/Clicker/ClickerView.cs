using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;
using DG.Tweening;

public class ClickerView : MonoBehaviour
{
    [SerializeField] private Button clickButton;
    [SerializeField] private ParticleSystem clickParticles;
    [SerializeField] private Transform coinPrefab;
    [SerializeField] private Transform currencyFlyTarget;
    [SerializeField] private TMPro.TextMeshProUGUI currencyText;
    [SerializeField] private TMPro.TextMeshProUGUI energyText;

    private ClickerController _clickerController;

    [Inject]
    public void Construct(ClickerController clickerController)
    {
        _clickerController = clickerController;
    }

    private void Start()
    {
        clickButton.OnClickAsObservable()
        .Subscribe(_ => _clickerController.OnClick())
        .AddTo(this);

        _clickerController.Currency
        .Subscribe(value => currencyText.text = $"Currency: {value}")
        .AddTo(this);

        _clickerController.Energy
        .Subscribe(value => energyText.text = $"Energy: {value}")
        .AddTo(this);
    }

    public void PlayClickVFX()
    {
        clickParticles?.Play();
    }

    public void SpawnCoin()
    {
        var currencyIcon = Instantiate(coinPrefab.gameObject, clickButton.transform.position, Quaternion.identity, transform);
        currencyIcon.transform.DOMove(currencyFlyTarget.position, 1f).OnComplete(() => Destroy(currencyIcon));
        currencyIcon.transform.GetChild(0).DOShakePosition(1, new Vector3(Random.Range(-50, 50), 0, 0), 4, 90, false, true);
    }
}