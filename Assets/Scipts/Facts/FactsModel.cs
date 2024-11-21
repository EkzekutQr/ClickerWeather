using UniRx;

public class FactsModel
{
    public ReactiveCollection<Fact> Facts { get; private set; } = new ReactiveCollection<Fact>();
    public ReactiveProperty<string> SelectedFactTitle { get; private set; } = new ReactiveProperty<string>();
    public ReactiveProperty<string> SelectedFactDetails { get; private set; } = new ReactiveProperty<string>();
}