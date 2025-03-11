using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    internal class HumanFK
    {
        private const float EPSILON = 0.001f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly SpringFloat m_UseRightSide = new SpringFloat(0f);
        [NonSerialized] private readonly SpringFloat m_UseLeftSide = new SpringFloat(0f);
        [NonSerialized] private readonly SpringFloat m_RatioSpine = new SpringFloat(0f);
        [NonSerialized] private readonly SpringFloat m_RatioLowerChest = new SpringFloat(0f);
        [NonSerialized] private readonly SpringFloat m_RatioUpperChest = new SpringFloat(0f);
        [NonSerialized] private readonly SpringFloat m_RatioShoulders = new SpringFloat(0f);
        [NonSerialized] private readonly SpringFloat m_RatioArms = new SpringFloat(0f);
        [NonSerialized] private readonly SpringFloat m_RatioHands = new SpringFloat(0f);
        [NonSerialized] private readonly SpringFloat m_RatioNeck = new SpringFloat(0f);
        [NonSerialized] private readonly SpringFloat m_RatioHead = new SpringFloat(0f);

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Update(THumanRotations humanBones, Args args, float decay, float deltaTime)
        {
            if (humanBones == null)
            {
                this.m_UseRightSide.Target = 0f;
                this.m_UseLeftSide.Target = 0f;
                this.m_RatioSpine.Target = 0f;
                this.m_RatioLowerChest.Target = 0f;
                this.m_RatioUpperChest.Target = 0f;
                this.m_RatioShoulders.Target = 0f;
                this.m_RatioArms.Target = 0f;
                this.m_RatioHands.Target = 0f;
                this.m_RatioNeck.Target = 0f;
                this.m_RatioHead.Target = 0f;
            }
            else
            {
                this.m_UseRightSide.Target = humanBones.GetUseRightSide(args);
                this.m_UseLeftSide.Target = humanBones.GetUseLeftSide(args);
                this.m_RatioSpine.Target = humanBones.GetRatioSpine(args);
                this.m_RatioLowerChest.Target = humanBones.GetRatioLowerChest(args);
                this.m_RatioUpperChest.Target = humanBones.GetRatioUpperChest(args);
                this.m_RatioShoulders.Target = humanBones.GetRatioShoulders(args);
                this.m_RatioArms.Target = humanBones.GetRatioArms(args);
                this.m_RatioHands.Target = humanBones.GetRatioHands(args);
                this.m_RatioNeck.Target = humanBones.GetRatioNeck(args);
                this.m_RatioHead.Target = humanBones.GetRatioHead(args);
            }
            
            this.m_UseRightSide.Update(decay, deltaTime);
            this.m_UseLeftSide.Update(decay, deltaTime);
            this.m_RatioSpine.Update(decay, deltaTime);
            this.m_RatioLowerChest.Update(decay, deltaTime);
            this.m_RatioUpperChest.Update(decay, deltaTime);
            this.m_RatioShoulders.Update(decay, deltaTime);
            this.m_RatioArms.Update(decay, deltaTime);
            this.m_RatioHands.Update(decay, deltaTime);
            this.m_RatioNeck.Update(decay, deltaTime);
            this.m_RatioHead.Update(decay, deltaTime);
        }
        
        public void RotateBody(Animator animator, Action<Transform, float> operation, float ratio)
        {
            Transform spine = animator.GetBoneTransform(HumanBodyBones.Spine);
            Transform lowerChest = animator.GetBoneTransform(HumanBodyBones.Chest);
            Transform upperChest = animator.GetBoneTransform(HumanBodyBones.UpperChest);
            Transform shoulderR = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
            Transform shoulderL = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            Transform armL = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            Transform armR = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
            Transform handL = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            Transform handR = animator.GetBoneTransform(HumanBodyBones.RightHand);
            Transform neck = animator.GetBoneTransform(HumanBodyBones.Neck);
            Transform head = animator.GetBoneTransform(HumanBodyBones.Head);
            
            float ratioSpine = this.m_RatioSpine.Current * ratio;
            float ratioLowerChest = this.m_RatioLowerChest.Current * ratio;
            float ratioUpperChest = this.m_RatioUpperChest.Current * ratio;
            float ratioShoulders = this.m_RatioShoulders.Current * ratio;
            float ratioArms = this.m_RatioArms.Current * ratio;
            float ratioHands = this.m_RatioHands.Current * ratio;
            float ratioNeck = this.m_RatioNeck.Current * ratio;
            float ratioHead = this.m_RatioHead.Current * ratio;
            float sideR = this.m_UseRightSide.Current * ratio;
            float sideL = this.m_UseLeftSide.Current * ratio;
            
            if (spine != null && ratioSpine > EPSILON) operation.Invoke(spine, ratioSpine);
            if (lowerChest != null && ratioLowerChest > EPSILON) operation.Invoke(lowerChest, ratioLowerChest);
            if (upperChest != null && ratioUpperChest > EPSILON) operation.Invoke(upperChest, ratioUpperChest);
            if (shoulderR != null && ratioShoulders > EPSILON) operation.Invoke(shoulderR, ratioShoulders * sideR);
            if (shoulderL != null && ratioShoulders > EPSILON) operation.Invoke(shoulderL, ratioShoulders * sideL);
            if (armL != null && ratioArms > EPSILON) operation.Invoke(armL, ratioArms * sideL);
            if (armR != null && ratioArms > EPSILON) operation.Invoke(armR, ratioArms * sideR);
            if (handL != null && ratioHands > EPSILON) operation.Invoke(handL, ratioHands * sideL);
            if (handR != null && ratioHands > EPSILON) operation.Invoke(handR, ratioHands * sideR);
            if (neck != null && ratioNeck > EPSILON) operation.Invoke(neck, ratioNeck);
            if (head != null && ratioHead > EPSILON) operation.Invoke(head, ratioHead);
        }
    }
}