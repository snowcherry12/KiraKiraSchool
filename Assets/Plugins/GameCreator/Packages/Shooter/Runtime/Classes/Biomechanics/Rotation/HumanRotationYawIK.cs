using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class HumanRotationYawIK : THumanRotations
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override float GetUseLeftSide(Args args) => 0f;
        public override float GetUseRightSide(Args args) => 0f;
        
        public override float GetRatioSpine(Args args)
        {
            return 0f;
        }

        public override float GetRatioLowerChest(Args args)
        {
            return 0f;
        }

        public override float GetRatioUpperChest(Args args)
        {
            return 0f;
        }

        public override float GetRatioShoulders(Args args)
        {
            return 0f;
        }

        public override float GetRatioArms(Args args)
        {
            return 0f;
        }

        public override float GetRatioHands(Args args)
        {
            return 0f;
        }

        public override float GetRatioNeck(Args args)
        {
            return 0f;
        }

        public override float GetRatioHead(Args args)
        {
            return 0f;
        }
    }
}