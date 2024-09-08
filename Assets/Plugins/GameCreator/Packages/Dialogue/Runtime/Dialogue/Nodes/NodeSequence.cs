using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class NodeSequence : Sequence
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private TimeMode m_TimeMode;
        
        [NonSerialized] private AnimationClip m_Animation;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override TimeMode TimeMode => this.m_TimeMode;
        
        public override float Duration => this.m_Animation != null ? this.m_Animation.length : 0f;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public NodeSequence(Track[] tracks) : base(tracks)
        { }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public async Task Run(TimeMode mode, AnimationClip animation, Args args)
        {
            this.m_TimeMode = mode;
            this.m_Animation = animation;
            
            await this.DoRun(args);
        }

        public void Cancel(Args args)
        {
            this.DoCancel(args);
        }
    }
}