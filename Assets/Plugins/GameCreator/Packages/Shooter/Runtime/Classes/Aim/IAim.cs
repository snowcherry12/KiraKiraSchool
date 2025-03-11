using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    public interface IAim
    {
        Vector3 GetPoint(Args args);
        
        void Enter(Character character);
        void Exit(Character character);
    }
}