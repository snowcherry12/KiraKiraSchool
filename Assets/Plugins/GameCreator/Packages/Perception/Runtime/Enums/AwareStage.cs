namespace GameCreator.Runtime.Perception
{
    public enum AwareStage
    {
        None       = 1 << 0,
        Suspicious = 1 << 1,
        Alert      = 1 << 2,
        Aware      = 1 << 3
    }
}