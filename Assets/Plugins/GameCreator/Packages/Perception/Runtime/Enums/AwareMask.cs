using System;

namespace GameCreator.Runtime.Perception
{
    [Flags]
    public enum AwareMask
    {
        None       = AwareStage.None,
        Suspicious = AwareStage.Suspicious,
        Alert      = AwareStage.Alert,
        Aware      = AwareStage.Aware
    }
}