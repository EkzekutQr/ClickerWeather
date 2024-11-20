using UniRx;
using UnityEngine;

public class TabModel
{
    public ReactiveProperty<string> ActiveTab { get; private set; } = new ReactiveProperty<string>("Clicker");

    public void SetActiveTab(string tabName)
    {
        ActiveTab.Value = tabName;
    }
}