using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class RunReloadSequence : TRun<ReloadSequence>
    {
        private const int PREWARM_COUNTER = 3;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private ReloadSequence m_Sequence = new ReloadSequence();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override ReloadSequence Value => this.m_Sequence;
        
        protected override GameObject Template
        {
            get
            {
                if (this.m_Template == null) this.m_Template = CreateTemplate(this.Value);
                return this.m_Template;
            }
        }

        // PUBLIC GETTERS: ------------------------------------------------------------------------

        public T GetTrack<T>() where T : ITrack
        {
            return this.m_Sequence.GetTrack<T>();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public async Task Run(
            string name,
            TimeMode time, 
            float speed, 
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
            
            RunnerReloadSequence runner = RunnerReloadSequence.Pick<RunnerReloadSequence>(
                template,
                config,
                PREWARM_COUNTER
            );
            
            if (runner == null) return;
            
            await runner.Value.Run(speed, time, animation, config.Cancellable, args);
            if (runner != null) RunnerReloadSequence.Restore(runner);
        }

        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static GameObject CreateTemplate(ReloadSequence value)
        {
            return RunnerReloadSequence.CreateTemplate<RunnerReloadSequence>(value);
        }
    }
}