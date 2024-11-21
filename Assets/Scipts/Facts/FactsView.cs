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
    [SerializeField] private TMPro.TextMeshProUGUI factDetailsPopupTextTitle;
    [SerializeField] private TMPro.TextMeshProUGUI factDetailsPopupTextBody;

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
        .Subscribe(fact => AddFactItem(fact.Value.Name, fact.Value.Id))
        .AddTo(this);

        _factsModel.SelectedFactDetails
        .Subscribe(details => ShowFactDetailsPopup(details))
        .AddTo(this);

        _factsModel.SelectedFactTitle
        .Subscribe(details => ShowFactDetailsPopupTitle(details))
        .AddTo(this);
    }

    private void AddFactItem(string factName, int factId)
    {
        var factItem = Instantiate(factItemPrefab, factsListContainer);
        factItem.GetComponent<FactItem>().FactName.text = factName;
        factItem.GetComponent<FactItem>().FactId.text = factId.ToString();
        factItem.GetComponent<Button>().OnClickAsObservable()
        .Subscribe(_ => OnFactSelected(factId))
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
        factDetailsPopupTextBody.text = details;
        factDetailsPopup.SetActive(true);
    }

    private void ShowFactDetailsPopupTitle(string details)
    {
        factDetailsPopupTextTitle.text = details;
        factDetailsPopup.SetActive(true);
    }

    public void HideFactDetailsPopup()
    {
        factDetailsPopup.SetActive(false);
    }
}