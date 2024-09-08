using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    internal class RuntimeStatusEffectData : ICancellable
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly float m_StartTime;
        [NonSerialized] private readonly float m_Duration;

        [NonSerialized] private readonly StatusEffect m_StatusEffect;
        
        [NonSerialized] private RunnerConfig m_ConfigWhileActive;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        private Args Args { get; }

        public bool IsCancelled { get; private set; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public RuntimeStatusEffectData(Traits traits, StatusEffect statusEffect, float elapsedTime = 0f)
        {
            this.Args = new Args(traits.gameObject);
            this.m_StartTime = Time.time - elapsedTime;
            this.m_Duration = statusEffect.GetDuration(this.Args);

            this.m_StatusEffect = statusEffect;
            
            _ = this.RunOnStart();
            _ = this.RunWhileActive();
        }

        // INTERNAL METHODS: ----------------------------------------------------------------------

        public bool Update()
        {
            if (!this.m_StatusEffect.HasDuration) return false;
            return Time.time > this.m_StartTime + this.m_Duration;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private async Task RunOnStart()
        {
            StatusEffect.LastAdded = this.m_StatusEffect;

            RunnerConfig config = new RunnerConfig
            {
                Name = $"On Start {TextUtils.Humanize(this.m_StatusEffect.name)}",
                Location = new RunnerLocationLocation(
                    this.Args.Self != null ? this.Args.Self.transform.position : Vector3.zero,
                    this.Args.Self != null ? this.Args.Self.transform.rotation : Quaternion.identity
                )
            };

            await this.m_StatusEffect.RunOnStart(this.Args.Clone, config);
        }
        
        private async Task RunOnEnd()
        {
            StatusEffect.LastRemoved = this.m_StatusEffect;

            RunnerConfig config = new RunnerConfig
            {
                Name = $"On End {TextUtils.Humanize(this.m_StatusEffect.name)}",
                Location = new RunnerLocationLocation(
                    this.Args.Self != null ? this.Args.Self.transform.position : Vector3.zero,
                    this.Args.Self != null ? this.Args.Self.transform.rotation : Quaternion.identity
                )
            };

            await this.m_StatusEffect.RunOnEnd(this.Args.Clone, config);
        }

        private async Task RunWhileActive()
        {
            RunnerConfig config = new RunnerConfig
            {
                Name = $"While Active {TextUtils.Humanize(this.m_StatusEffect.name)}",
                Location = new RunnerLocationLocation(
                    this.Args.Self != null ? this.Args.Self.transform.position : Vector3.zero,
                    this.Args.Self != null
                        ? this.Args.Self.transform.rotation
                        : Quaternion.identity
                ),
                Cancellable = this
            };
            
            while (!this.IsCancelled && !ApplicationManager.IsExiting)
            {
                int frame = Time.frameCount;
                
                await this.m_StatusEffect.RunWhileActive(this.Args.Clone, config);
                if (frame == Time.frameCount) await Task.Yield();
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Stop()
        {
            this.IsCancelled = true;
            _ = this.RunOnEnd();
        }

        public RuntimeStatusEffectValue GetValue()
        {
            float timeElapsed = Time.time - this.m_StartTime;

            if (this.m_StatusEffect.HasDuration)
            {
                return new RuntimeStatusEffectValue(
                    this.m_StatusEffect.ID.String,
                    timeElapsed, this.m_StatusEffect.GetDuration(this.Args) - timeElapsed
                );
            }
            
            return new RuntimeStatusEffectValue(
                this.m_StatusEffect.ID.String,
                timeElapsed, -1f
            );
        }
    }
}