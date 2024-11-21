using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;
using DG.Tweening;

public class ClickerButton : MonoBehaviour
{
    [SerializeField] private Button clickButton;
    [SerializeField] private ParticleSystem clickParticles;
    [SerializeField] private Transform coinPrefab;
    [SerializeField] private Transform currencyFlyTarget;

    private IClickerController _clickerController;

    [Inject]
    public void Construct(IClickerController clickerController)
    {
        _clickerController = clickerController;
    }

    private void Start()
    {
        clickButton.OnClickAsObservable()
        .Subscribe(_ => _clickerController.OnClick())
        .AddTo(this);
    }

    public void PlayClickVFX()
    {
        if (clickParticles == null) return;

        clickParticles.Play();
    }

    public void SpawnCoin()
    {
        var currencyIcon = Instantiate(coinPrefab.gameObject, clickButton.transform.position, Quaternion.identity, transform);
        currencyIcon.transform.DOMove(currencyFlyTarget.position, 1f).OnComplete(() => Destroy(currencyIcon));
        currencyIcon.transform.GetChild(0).DOShakePosition(1, new Vector3(Random.Range(-50, 50), 0, 0), 4, 90, false, true);
    }
}