using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class HumanRotationPitchFK : THumanRotations
    {
        // EXPOSED METHODS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetBool m_UseLeftSide = GetBoolFalse.Create;
        [SerializeField] private PropertyGetBool m_UseRightSide = GetBoolTrue.Create;
        
        [SerializeField] private PropertyGetDecimal m_Spine = GetDecimalDecimal.Create(0.1f);
        [SerializeField] private PropertyGetDecimal m_LowerChest = GetDecimalDecimal.Create(0.1f);
        [SerializeField] private PropertyGetDecimal m_UpperChest = GetDecimalDecimal.Create(0.1f);
        [SerializeField] private PropertyGetDecimal m_Shoulders = GetDecimalDecimal.Create(0.4f);
        [SerializeField] private PropertyGetDecimal m_Arms = GetDecimalDecimal.Create(0.2f);
        [SerializeField] private PropertyGetDecimal m_Hands = GetDecimalDecimal.Create(0.1f);
        [SerializeField] private PropertyGetDecimal m_Neck = GetDecimalDecimal.Create(0.2f);
        [SerializeField] private PropertyGetDecimal m_Head = GetDecimalDecimal.Create(0.2f);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override float GetUseLeftSide(Args args) => this.m_UseLeftSide.Get(args) ? 1f : 0f;
        public override float GetUseRightSide(Args args) => this.m_UseRightSide.Get(args) ? 1f : 0f;
        
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

        public override float GetRatioShoulders(Args args)
        {
            return (float) this.m_Shoulders.Get(args);
        }

        public override float GetRatioArms(Args args)
        {
            return (float) this.m_Arms.Get(args);
        }

        public override float GetRatioHands(Args args)
        {
            return (float) this.m_Hands.Get(args);
        }

        public override float GetRatioNeck(Args args)
        {
            return (float) this.m_Neck.Get(args);
        }

        public override float GetRatioHead(Args args)
        {
            return (float) this.m_Head.Get(args);
        }
    }
}