using UniRx;
using Cysharp.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Zenject;
using System.Threading;
using UnityEngine;

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
                var factName = fact["name"]?.Value<string>();
                var factId = fact["id"]?.Value<int>();

                if (!string.IsNullOrEmpty(factName) && factId.HasValue)
                {
                    _factsModel.Facts.Add(new Fact { Name = factName, Id = factId.Value });
                }
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
            Debug.Log(response);
            var factDetails = JObject.Parse(response);

            var name = factDetails["name"]?.Value<string>();
            var bredFor = factDetails["bred_for"]?.Value<string>();
            var lifeSpan = factDetails["life_span"]?.Value<string>();
            var temperament = factDetails["temperament"]?.Value<string>();

            var title = $"Name: { name}";
            var details = $"Bred For: {bredFor}\nLife Span: {lifeSpan}\nTemperament: {temperament}";
            _factsModel.SelectedFactTitle.Value = title;
            _factsModel.SelectedFactDetails.Value = details;
        }
        _factsView.ShowLoadingIndicator(false);
    }
}
public class Fact
{
    public string Name { get; set; }
    public int Id { get; set; }
}