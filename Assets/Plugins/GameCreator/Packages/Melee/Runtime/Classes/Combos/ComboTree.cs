using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class ComboTree : TSerializableTree<ComboItem>, ISerializationCallbackReceiver
    {
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool IsRoot(int nodeId)
        {
            return this.m_Roots.Contains(nodeId);
        }
        
        public ComboItemParent[] GetParentData(int comboId)
        {
            List<ComboItemParent> parents = new List<ComboItemParent>();
            comboId = this.Parent(comboId);
            
            while (comboId != NODE_INVALID)
            {
                ComboItem parent = this.Get(comboId);
                parents.Insert(0, new ComboItemParent(parent.Key, parent.Mode, parent.When));
                
                comboId = this.Parent(comboId);
            }

            return parents.ToArray();
        }

        public ChargeMatch MatchCharge(int previousComboId, Input input, Args args)
        {
            if (previousComboId != NODE_INVALID)
            {
                foreach (int rootId in this.m_Roots)
                {
                    ComboItem candidate = this.Get(rootId);
                    
                    if (candidate.When == MeleeExecute.InOrder) continue;
                    if (previousComboId == rootId) continue;
                    
                    if (!candidate.CheckConsumeCharge(input, args)) continue;
                    if (!candidate.CheckConditions(args)) continue;
                    
                    return new ChargeMatch(previousComboId, rootId);
                }
            }
            
            List<int> candidateIds = this.GetCandidates(previousComboId);
            foreach (int candidateId in candidateIds)
            {
                ComboItem candidate = this.Get(candidateId);
                if (!candidate.CheckConsumeCharge(input, args)) continue;
                if (!candidate.CheckConditions(args)) continue;
                
                return new ChargeMatch(previousComboId, candidateId);
            }
            
            return new ChargeMatch(previousComboId, NODE_INVALID);
        }
        
        public int MatchExecuteCharge(int currentComboId, Input input, float chargeDuration, Args args)
        {
            ComboItem candidate = this.Get(currentComboId);
                
            if (!candidate.CheckConsumeExecuteCharge(input, chargeDuration, args)) return NODE_INVALID;
            if (!candidate.CheckConditions(args)) return NODE_INVALID;
            
            return currentComboId;
        }
        
        public int MatchExecuteTap(int previousComboId, Input input, Args args)
        {
            if (previousComboId != NODE_INVALID)
            {
                foreach (int rootId in this.m_Roots)
                {
                    ComboItem candidate = this.Get(rootId);
                    
                    if (candidate.When == MeleeExecute.InOrder) continue;
                    if (previousComboId == rootId) continue;
                    
                    if (!candidate.CheckConsumeExecuteTap(input, args)) continue;
                    if (!candidate.CheckConditions(args)) continue;
                    
                    return rootId;
                }
            }
            
            List<int> candidateIds = this.GetCandidates(previousComboId);
            
            foreach (int candidateId in candidateIds)
            {
                ComboItem candidate = this.Get(candidateId);
                
                if (!candidate.CheckConsumeExecuteTap(input, args)) continue;
                if (!candidate.CheckConditions(args)) continue;
                
                return candidateId;
            }
        
            return NODE_INVALID;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private List<int> GetCandidates(int previousComboId)
        {
            return previousComboId == NODE_INVALID
                ? this.m_Roots
                : this.Children(previousComboId);
        }
        
        // SERIALIZATION: -------------------------------------------------------------------------

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (AssemblyUtils.IsReloading) return;
            
            foreach (int rootId in this.m_Roots)
            {
                this.m_Data[rootId].Value.OnBeforeSerializeRoot();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        { }
    }
}
