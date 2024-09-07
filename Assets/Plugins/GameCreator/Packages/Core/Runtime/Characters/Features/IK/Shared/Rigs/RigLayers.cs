using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters.IK
{
    [Serializable]
    public class RigLayers : TPolymorphicList<TRig>
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeReference] protected List<TRig> m_Rigs = new List<TRig>();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override int Length => this.m_Rigs.Count;
        
        [field: NonSerialized] private Character Character { get; set; }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public T GetRig<T>() where T : TRig
        {
            foreach (TRig rig in this.m_Rigs) if (rig is T tRig) return tRig;
            return null;
        }
        
        public T Create<T>() where T : TRig, new()
        {
            if (this.Character == null) return null;

            T rig = new T();
            this.m_Rigs.Add(rig);
            
            rig.OnStartup(this.Character);
            if (this.Character.isActiveAndEnabled) rig.OnEnable(this.Character);
            
            return rig;
        }
        
        // IK METHODS: ----------------------------------------------------------------------------

        public void OnStartup(InverseKinematics inverseKinematics)
        {
            this.Character = inverseKinematics.Character;
            
            foreach (TRig rig in this.m_Rigs)
            {
                if (!this.Character.Animim.Animator.isHuman && rig.RequiresHuman) continue;
                rig?.OnStartup(this.Character);
            }
        }
        
        public void OnEnable()
        {
            foreach (TRig rig in this.m_Rigs)
            {
                if (!this.Character.Animim.Animator.isHuman && rig.RequiresHuman) continue;
                rig?.OnEnable(this.Character);   
            }
        }

        public void OnDisable()
        {
            if (this.Character.Animim?.Animator == null) return;

            foreach (TRig rig in this.m_Rigs)
            {
                if (!this.Character.Animim.Animator.isHuman && rig.RequiresHuman) continue;
                rig?.OnDisable(this.Character);
            }
        }

        public void OnUpdate()
        {
            foreach (TRig rig in this.m_Rigs)
            {
                if (!this.Character.Animim.Animator.isHuman && rig.RequiresHuman) continue;
                rig?.OnUpdate(this.Character);   
            }
        }

        public void OnDrawGizmos()
        {
            foreach (TRig rig in this.m_Rigs)
            {
                if (!this.Character.Animim.Animator.isHuman && rig.RequiresHuman) continue;
                rig?.OnDrawGizmos(this.Character);   
            }
        }
    }
}
