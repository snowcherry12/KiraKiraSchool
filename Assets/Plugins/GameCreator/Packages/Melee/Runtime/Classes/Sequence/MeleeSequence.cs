using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class MeleeSequence : Sequence
    {
        public const int TRACK_PHASES = 0;
        public const int TRACK_CANCEL = 1;
        public const int TRACK_ROOT_MOTION_POSITION = 2;
        public const int TRACK_ROOT_MOTION_ROTATION = 3;
        public const int TRACK_MOTION_WARPING = 4;
        public const int TRACK_INSTRUCTIONS = 5;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private TimeMode m_TimeMode;
        [NonSerialized] private AttackSpeed m_Speed;
        [NonSerialized] private AttackDuration m_Duration;
        [NonSerialized] private ICancellable m_Cancellable;
        
        [NonSerialized] private AnimationClip m_Animation;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override TimeMode TimeMode => this.m_TimeMode;
        
        public override float Duration => this.m_Animation != null ? m_Duration.Total : 0f;

        protected override ICancellable CancellationToken => this.m_Cancellable;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public MeleeSequence() : base(new Track[]
        {
            new TrackMeleePhases(),
            new TrackMeleeCancel(),
            new TrackMeleeRootMotionPosition(),
            new TrackMeleeRootMotionRotation(),
            new TrackMeleeMotionWarping(),
            new TrackDefault()
        }) { }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public async Task Run(
            TimeMode mode, 
            AttackSpeed speed, 
            AttackDuration duration,
            AnimationClip animation,
            ICancellable cancellable,
            Args args)
        {
            this.m_TimeMode = mode;
            this.m_Speed = speed;
            this.m_Duration = duration;
            this.m_Cancellable = cancellable;
            
            this.m_Animation = animation;
            await this.DoRun(args);
        }

        public void Cancel(Args args)
        {
            this.DoCancel(args);
        }

        public AttackRatio GetPhasesRatios()
        {
            TrackMeleePhases track = this.GetTrack<TrackMeleePhases>();
        
            if (track == null || track.Clips.Length == 0)
            {
                return new AttackRatio(1f, 0f, 0f);
            }
        
            if (track.Clips[0] is not ClipMeleePhases phases)
            {
                return new AttackRatio(1f, 0f, 0f);
            }
        
            return new AttackRatio(
                phases.DurationToStart,
                phases.Duration,
                phases.DurationToEnd
            );
        }
        
        public AttackRatio GetPhasesDilatedRatios(AttackSpeed speed)
        {
            TrackMeleePhases track = this.GetTrack<TrackMeleePhases>();
        
            if (track == null || track.Clips.Length == 0)
            {
                return new AttackRatio(1f, 0f, 0f);
            }
        
            if (track.Clips[0] is not ClipMeleePhases phases)
            {
                return new AttackRatio(1f, 0f, 0f);
            }
        
            float anticipation = speed.Anticipation > 0f 
                ? phases.DurationToStart / speed.Anticipation 
                : 0f;
        
            float strike = speed.Strike > 0f
                ? phases.Duration / speed.Strike
                : 0f;
        
            float recovery = speed.Recovery > 0f
                ? phases.DurationToEnd / speed.Recovery
                : 0f;
            
            float total = anticipation + strike + recovery;
            if (total <= 0f) return new AttackRatio(0f, 0f, 0f);
            
            return new AttackRatio(
                anticipation / total,
                strike / total,
                recovery / total
            );
        }

        public float GetDilated(float t, AttackSpeed speed)
        {
            AttackRatio ratioN = this.GetPhasesRatios();
            AttackRatio ratioD = this.GetPhasesDilatedRatios(speed);
        
            float rnA = ratioN.Anticipation;
            float rnS = ratioN.Strike;
            float rnR = ratioN.Recovery;
        
            float ta = Math.Min(Math.Max(t, 0f), rnA);
            float ts = Math.Min(Math.Max(t - rnA, 0f), rnS);
            float tr = Math.Min(Math.Max(t - (rnA + rnS), 0f), rnR);

            float dilation = 0f;

            if (rnA > 0f) dilation += ta / rnA * ratioD.Anticipation;
            if (rnS > 0f) dilation += ts / rnS * ratioD.Strike;
            if (rnR > 0f) dilation += tr / rnR * ratioD.Recovery;

            return dilation;
        }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override float GetDilated(float t)
        {
            return this.GetDilated(t, this.m_Speed);
        }
    }
}