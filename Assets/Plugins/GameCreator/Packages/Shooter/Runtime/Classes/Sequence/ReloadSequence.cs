using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class ReloadSequence : Sequence
    {
        public const int TRACK_MAGAZINE = 0;
        public const int TRACK_QUICK = 1;
        public const int TRACK_INSTRUCTIONS = 2;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private TimeMode m_TimeMode;
        
        [NonSerialized] private ICancellable m_Cancellable;
        [NonSerialized] private AnimationClip m_Animation;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override TimeMode TimeMode => this.m_TimeMode;
        
        public override float Duration => this.m_Animation != null 
            ? this.m_Animation.length / this.Speed 
            : 0f;

        protected override ICancellable CancellationToken => this.m_Cancellable;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ReloadSequence() : base(new Track[]
        {
            new TrackReloadMagazine(),
            new TrackReloadQuick(),
            new TrackDefault()
        }) { }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public async Task Run(
            float speed, 
            TimeMode mode, 
            AnimationClip animation,
            ICancellable cancellable,
            Args args)
        {
            this.Speed = speed;
            this.m_TimeMode = mode;
            this.m_Cancellable = cancellable;
            
            this.m_Animation = animation;
            await this.DoRun(args);
        }

        public void Cancel(Args args)
        {
            this.DoCancel(args);
        }
    }
}