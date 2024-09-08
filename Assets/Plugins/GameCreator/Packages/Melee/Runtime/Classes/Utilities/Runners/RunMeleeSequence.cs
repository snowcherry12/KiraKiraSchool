using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class RunMeleeSequence : TRun<MeleeSequence>
    {
        private const int PREWARM_COUNTER = 3;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private MeleeSequence m_Sequence;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override MeleeSequence Value => this.m_Sequence;
        
        protected override GameObject Template
        {
            get
            {
                if (this.m_Template == null) this.m_Template = CreateTemplate(this.Value);
                return this.m_Template;
            }
        }

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public RunMeleeSequence()
        {
            this.m_Sequence = new MeleeSequence();
        }
        
        // PUBLIC GETTERS: ------------------------------------------------------------------------

        public T GetTrack<T>() where T : ITrack
        {
            return this.m_Sequence.GetTrack<T>();
        }

        public float GetDilated(float t, AttackSpeed speed)
        {
            return this.m_Sequence.GetDilated(t, speed);
        }
        
        public AttackRatio GetPhasesDilatedRatios(AttackSpeed speeds)
        {
            return this.m_Sequence.GetPhasesDilatedRatios(speeds);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public async Task Run(
            string name,
            TimeMode time, 
            AttackSpeed speed, 
            AttackDuration duration, 
            AnimationClip animation,
            ICancellable token, 
            Args args)
        {
            GameObject template = this.Template;
            RunnerConfig config = new RunnerConfig
            {
                Name = name,
                Cancellable = token
            };
            
            RunnerMeleeSequence runner = RunnerMeleeSequence.Pick<RunnerMeleeSequence>(
                template,
                config,
                PREWARM_COUNTER
            );
            
            if (runner == null) return;
            
            await runner.Value.Run(time, speed, duration, animation, config.Cancellable, args);
            if (runner != null) RunnerMeleeSequence.Restore(runner);
        }

        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static GameObject CreateTemplate(MeleeSequence value)
        {
            return RunnerMeleeSequence.CreateTemplate<RunnerMeleeSequence>(value);
        }
    }
}