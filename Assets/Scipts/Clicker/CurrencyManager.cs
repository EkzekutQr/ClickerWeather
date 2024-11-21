using UnityEngine;
using UniRx;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] int currency = 0;
    public ReactiveProperty<int> Currency { get; private set; } = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> Energy { get; private set; } = new ReactiveProperty<int>(1000);

    private void Start()
    {
        // Автосбор валюты каждые 3 секунды
        Observable.Interval(System.TimeSpan.FromSeconds(3))
        .Where(_ => Energy.Value > 0)
        .Subscribe(_ => CollectCurrency())
        .AddTo(this);

        // Начисление энергии каждые 10 секунд
        Observable.Interval(System.TimeSpan.FromSeconds(10))
        .Subscribe(_ => AddEnergy(10))
        .AddTo(this);
    }

    private void Update()
    {
        currency = Currency.Value;
    }

    public void CollectCurrency()
    {
        if (Energy.Value > 0)
        {
            Currency.Value += 1;
            Energy.Value -= 1;
        }
    }

    public void AddEnergy(int amount)
    {
        Energy.Value = Mathf.Min(Energy.Value + amount, 1000);
    }

    public void OnButtonClick()
    {
        if (Energy.Value > 0)
        {
            Currency.Value += 1;
            Energy.Value -= 1;
        }
    }
}