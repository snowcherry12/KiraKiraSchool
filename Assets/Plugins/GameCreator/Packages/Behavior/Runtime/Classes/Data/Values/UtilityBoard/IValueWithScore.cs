namespace GameCreator.Runtime.Behavior
{
    public interface IValueWithScore : IValue
    {
        float Score { get; set; }
    }
}