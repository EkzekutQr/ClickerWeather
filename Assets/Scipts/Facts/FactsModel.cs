using UniRx;

public class FactsModel
{
    public ReactiveCollection<string> Facts { get; private set; } = new ReactiveCollection<string>();
    public ReactiveProperty<string> SelectedFactDetails { get; private set; } = new ReactiveProperty<string>();
}