using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class ReloadList : TPolymorphicList<ReloadItem>
    {
        private const float TRANSITION = 0.25f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private StateData m_State = new StateData(StateData.StateType.State);
        [SerializeField] private PropertyGetInteger m_Layer = GetDecimalInteger.Create(9);
        
        [SerializeReference] private ReloadItem[] m_Reloads = 
        {
            new ReloadItem()
        };
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override int Length => this.m_Reloads.Length;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public async Task EnterState(Character character, float speed, Args args)
        {
            if (this.m_State.IsValid(character) == false) return;
            
            ConfigState configuration = new ConfigState(0f, speed, 1f, TRANSITION, 0f);
            
            _ = character.States.SetState(
                this.m_State, (int) this.m_Layer.Get(args), 
                BlendMode.Blend, configuration
            );

            float startTime = character.Time.Time;
            float duration = this.m_State.EntryDuration / speed;
            
            if (duration == 0f) return;
            
            while (character != null && character.Time.Time - startTime <= duration)
            {
                if (AsyncManager.ExitRequest) return;
                await Task.Yield();
            }
        }
        
        public async Task ExitState(Character character, float speed, CancelReloadSequence cancel, Args args)
        {
            if (this.m_State.IsValid(character) == false) return;
            
            int layer = (int) this.m_Layer.Get(args);
            float transition = cancel.CancelReason == CancelReason.ForceStop ? 0f : TRANSITION;
            
            if (character == null) return;
            character.States.Stop(layer, 0f, transition);
            
            float startTime = character.Time.Time;
            float duration = this.m_State.ExitDuration / speed;
            
            if (duration == 0f) return;
            if (cancel.CancelReason == CancelReason.ForceStop) return;
            
            while (character != null && character.Time.Time - startTime <= duration)
            {
                if (AsyncManager.ExitRequest) return;
                await Task.Yield();
            }
        }
        
        public Reload Pick(Character character, bool isFull)
        {
            foreach (ReloadItem reloadItem in this.m_Reloads)
            {
                if (reloadItem.Check(character, isFull) == false) continue;
                return reloadItem.Asset;
            }

            return null;
        }
    }
}