using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class HumanRotationLean : THumanRotations
    {
        [SerializeField] private PropertyGetDecimal m_Spine = GetDecimalConstantPointFive.Create;
        [SerializeField] private PropertyGetDecimal m_LowerChest = GetDecimalConstantPointFive.Create;
        [SerializeField] private PropertyGetDecimal m_UpperChest = GetDecimalConstantZero.Create;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override float GetUseLeftSide(Args args) => 0f;
        public override float GetUseRightSide(Args args) => 0f;
        
        public override float GetRatioSpine(Args args)
        {
            return (float) this.m_Spine.Get(args);
        }

        public override float GetRatioLowerChest(Args args)
        {
            return (float) this.m_LowerChest.Get(args);
        }

        public override float GetRatioUpperChest(Args args)
        {
            return (float) this.m_UpperChest.Get(args);
        }

        public override float GetRatioShoulders(Args args) => 0f;

        public override float GetRatioArms(Args args) => 0f;

        public override float GetRatioHands(Args args) => 0f;

        public override float GetRatioNeck(Args args) => 0f;

        public override float GetRatioHead(Args args) => 0f;
    }
}