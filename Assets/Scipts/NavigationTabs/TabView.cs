using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Zenject;
using System;

public class TabView : MonoBehaviour
{
    [SerializeField] private GameObject clickerPanel;
    [SerializeField] private GameObject factsPanel;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Button clickerButton;
    [SerializeField] private Button factsButton;

    private TabModel _model;

    [Inject]
    public void Construct(TabModel model)
    {
        _model = model;
        _model.ActiveTab.Subscribe(tab =>
        {
            clickerPanel.SetActive(tab == "Clicker");
            factsPanel.SetActive(tab == "Facts");
        }).AddTo(this);
    }

    public IObservable<Unit> OnClickerButtonClicked()
    {
        return clickerButton.OnClickAsObservable();
    }

    public IObservable<Unit> OnFactsButtonClicked()
    {
        return factsButton.OnClickAsObservable();
    }

    public async UniTask PlayFadeOutAnimation()
    {
        fadeImage.gameObject.SetActive(true);
        await fadeImage.DOFade(1f, 0.5f).AsyncWaitForCompletion();
    }

    public async UniTask PlayFadeInAnimation()
    {
        await fadeImage.DOFade(0f, 0.5f).AsyncWaitForCompletion();
        fadeImage.gameObject.SetActive(false);
    }

    public void SetButtonsInteractable(bool interactable)
    {
        clickerButton.interactable = interactable;
        factsButton.interactable = interactable;
    }
}