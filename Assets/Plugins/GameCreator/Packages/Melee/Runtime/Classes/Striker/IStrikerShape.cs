using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    public interface IStrikerShape
    {
        void Start(Transform transform);
        void Stop(Transform transform);
        
        List<StrikeOutput> Collect(Transform transform, LayerMask layerMask, int predictions);

        void OnDrawGizmos(Transform transform);
    }
}