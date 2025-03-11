using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("Aim")]
    
    [Serializable]
    public abstract class TAim : TPolymorphicItem<TAim>, IAim
    {
        // GETTER METHODS: ------------------------------------------------------------------------

        public abstract Vector3 GetPoint(Args args);
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual void Enter(Character character)
        { }

        public virtual void Exit(Character character)
        { }
    }
}