using UniRx;
using Cysharp.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Zenject;
using System.Threading;

public class FactsController
{
    private readonly FactsModel _factsModel;
    private readonly RequestQueue _requestQueue;
    private readonly FactsView _factsView;
    private CancellationTokenSource _cancellationTokenSource;

    [Inject]
    public FactsController(FactsModel factsModel, RequestQueue requestQueue, FactsView factsView)
    {
        _factsModel = factsModel;
        _requestQueue = requestQueue;
        _factsView = factsView;
    }

    public void OnFactsTabSelected()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        _requestQueue.EnqueueRequest(GetFactsAsync(token));
    }

    private async UniTask GetFactsAsync(CancellationToken token)
    {
        _factsView.ShowLoadingIndicator(true);
        using (var client = new HttpClient())
        {
            var response = await client.GetStringAsync("https://api.thedogapi.com/v1/breeds");
            var factsData = JArray.Parse(response);

            _factsModel.Facts.Clear();
            foreach (var fact in factsData)
            {
                _factsModel.Facts.Add(fact["name"].Value<string>());
            }
        }
        _factsView.ShowLoadingIndicator(false);
    }

    public void OnFactSelected(int factId)
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        _requestQueue.EnqueueRequest(GetFactDetailsAsync(factId, token));
    }

    private async UniTask GetFactDetailsAsync(int factId, CancellationToken token)
    {
        _factsView.ShowLoadingIndicator(true);
        using (var client = new HttpClient())
        {
            var response = await client.GetStringAsync($"https://api.thedogapi.com/v1/breeds/{factId}");
            var factDetails = JObject.Parse(response);

            _factsModel.SelectedFactDetails.Value = factDetails["description"].Value<string>();
        }
        _factsView.ShowLoadingIndicator(false);
    }
}