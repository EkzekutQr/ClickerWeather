using UniRx;
using Cysharp.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Zenject;
using System.Threading;
using UnityEngine;
using System;
using System.Collections.Generic;

public class WeatherController
{
    private readonly WeatherModel _weatherModel;
    private readonly RequestQueue _requestQueue;
    private CancellationTokenSource _cancellationTokenSource;
    private bool _isClickerScreenActive;
    private readonly List<IDisposable> _subscriptions = new List<IDisposable>();

    [Inject]
    public WeatherController(WeatherModel weatherModel, RequestQueue requestQueue)
    {
        _weatherModel = weatherModel;
        _requestQueue = requestQueue;
        SetClickerScreenActive(true); // Обеспечиваем, что обновления начнутся сразу
        StartWeatherUpdates();
    }

    public void SetClickerScreenActive(bool isActive)
    {
        Debug.Log($"SetClickerScreenActive called with isActive: {isActive}");
        _isClickerScreenActive = isActive;
        if (!isActive)
        {
            _cancellationTokenSource?.Cancel();
        }
    }

    public void StartWeatherUpdates()
    {
        Debug.Log("StartWeatherUpdates called");
        var subscription = Observable.Interval(System.TimeSpan.FromSeconds(5))
        .Where(_ => _isClickerScreenActive)
        .Subscribe(_ => EnqueueWeatherRequest());

        _subscriptions.Add(subscription);
    }

    private void EnqueueWeatherRequest()
    {
        Debug.Log("EnqueueWeatherRequest called");
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        _requestQueue.EnqueueRequest(GetWeatherAsync(token));
    }

    private async UniTask GetWeatherAsync(CancellationToken token)
    {
        using (var client = new HttpClient())
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.weather.gov/gridpoints/TOP/32,81/forecast");
                request.Headers.Add("User-Agent", "UnityApp");

                var response = await client.SendAsync(request, token);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log("Response received: " + responseBody);

                var weatherData = JObject.Parse(responseBody);
                Debug.Log("Parsed JSON: " + weatherData.ToString());

                var periods = weatherData["properties"]?["periods"];
                if (periods != null && periods.HasValues)
                {
                    var firstPeriod = periods.First;
                    var temperature = firstPeriod?["temperature"]?.Value<int>() ?? 0;
                    var icon = firstPeriod?["icon"]?.Value<string>() ?? string.Empty;

                    Debug.Log($"Parsed temperature: {temperature}, icon: {icon}");

                    _weatherModel.Temperature.Value = temperature;
                    _weatherModel.WeatherIcon.Value = icon;
                }
                else
                {
                    Debug.LogError("Не удалось получить данные о погоде.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Ошибка при получении данных о погоде: " + ex.Message);
            }
        }
    }
}