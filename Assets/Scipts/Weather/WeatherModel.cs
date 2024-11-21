using UniRx;

public class WeatherModel
{
    public ReactiveProperty<string> WeatherIcon { get; private set; } = new ReactiveProperty<string>();
    public ReactiveProperty<int> Temperature { get; private set; } = new ReactiveProperty<int>();
}
