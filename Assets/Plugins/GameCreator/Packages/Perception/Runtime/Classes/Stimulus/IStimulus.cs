using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    public interface IStimulus
    {
        string Tag { get; }
        Vector3 Position { get; }
        float Radius { get; }
        float Intensity { get; }
    }
}