using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;


public class FactsView : MonoBehaviour
{
    [SerializeField] private Transform factsListContainer;
    [SerializeField] private GameObject factItemPrefab;
    [SerializeField] private GameObject loadingIndicator;
    [SerializeField] private GameObject factDetailsPopup;
    [SerializeField] private TMPro.TextMeshProUGUI factDetailsPopupText;

    private FactsModel _factsModel;
    private FactsController _factsController;

    [Inject]
    public void Construct(FactsModel factsModel, FactsController factsController)
    {
        _factsModel = factsModel;
        _factsController = factsController;
    }

    private void Start()
    {
        _factsModel.Facts.ObserveAdd()
        .Subscribe(fact => AddFactItem(fact.Value, fact.Index))
        .AddTo(this);

        _factsModel.SelectedFactDetails
        .Subscribe(details => ShowFactDetailsPopup(details))
        .AddTo(this);
    }

    private void AddFactItem(string fact, int index)
    {
        var factItem = Instantiate(factItemPrefab, factsListContainer);
        factItem.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"{index + 1} - {fact}";
        factItem.GetComponent<Button>().OnClickAsObservable()
        .Subscribe(_ => OnFactSelected(index))
        .AddTo(this);
    }

    private void OnFactSelected(int factId)
    {
        _factsController.OnFactSelected(factId);
    }

    public void ShowLoadingIndicator(bool show)
    {
        loadingIndicator.SetActive(show);
    }

    private void ShowFactDetailsPopup(string details)
    {
        factDetailsPopupText.text = details;
        factDetailsPopup.SetActive(true);
    }

    public void HideFactDetailsPopup()
    {
        factDetailsPopup.SetActive(false);
    }
}