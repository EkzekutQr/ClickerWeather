using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;
using UnityEngine.Networking;
using System.Collections;
using System;
using Cysharp.Threading.Tasks;

public class WeatherView : MonoBehaviour
{
    [SerializeField] private Image weatherIcon;
    [SerializeField] private TMPro.TextMeshProUGUI temperatureText;

    private WeatherModel _weatherModel;

    [Inject]
    public void Construct(WeatherModel weatherModel)
    {
        _weatherModel = weatherModel;
    }

    private void Start()
    {
        _weatherModel.WeatherIcon
        .Subscribe(icon => UpdateWeatherIcon(icon))
        .AddTo(this);

        _weatherModel.Temperature
        .Subscribe(temp => temperatureText.text = $"Сегодня - {temp}F")
        .AddTo(this);
    }

    private void UpdateWeatherIcon(string iconUrl)
    {
        Debug.Log("Updating weather icon with URL: " + iconUrl);
        if (Uri.IsWellFormedUriString(iconUrl, UriKind.Absolute))
        {
            LoadIcon(iconUrl).Forget();
        }
        else
        {
            Debug.LogError("Malformed URL: " + iconUrl);
        }
    }

    private async UniTaskVoid LoadIcon(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            await uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                var texture = DownloadHandlerTexture.GetContent(uwr);
                weatherIcon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                Debug.Log("Weather icon updated successfully.");
            }
            else
            {
                Debug.LogError("Не удалось загрузить иконку погоды: " + uwr.error);
            }
        }
    }
}