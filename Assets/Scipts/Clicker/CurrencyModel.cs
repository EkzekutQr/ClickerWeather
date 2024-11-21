using UnityEngine;
using UniRx;

public class CurrencyModel
{
    public ReactiveProperty<int> Currency { get; private set; } = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> Energy { get; private set; } = new ReactiveProperty<int>(1000);

    public void AddCurrency(int amount)
    {
        Currency.Value += amount;
    }

    public void SpendEnergy(int amount)
    {
        Energy.Value = Mathf.Max(Energy.Value - amount, 0);
    }

    public void AddEnergy(int amount, int maxEnergy)
    {
        Energy.Value = Mathf.Min(Energy.Value + amount, maxEnergy);
    }
}