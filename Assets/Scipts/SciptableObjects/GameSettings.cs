using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
public class GameSettings : ScriptableObject
{
    public int initialEnergy = 1000;
    public int energyPerInterval = 10;
    public int maxEnergy = 1000;
    public int currencyPerClick = 1;
    public int energyCostPerClick = 1;
    public int energyCostPerAutoCollect = 1;
}