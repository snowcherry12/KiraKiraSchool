using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    internal class RuntimeStatusEffectList
    {
        // MEMBERS: -------------------------------------------------------------------------------

        private readonly List<RuntimeStatusEffectData> m_List = new List<RuntimeStatusEffectData>();

        private readonly Traits m_Traits;
        private readonly StatusEffect m_StatusEffect;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public int Count => this.m_List.Count;
        
        public IdString ID => this.m_StatusEffect.ID;
        public StatusEffect StatusEffect => this.m_StatusEffect;
        
        private Args Args { get; }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action<IdString> EventChange;
        
        public event Action<IdString> EventAdd;
        public event Action<IdString> EventRemove;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public RuntimeStatusEffectList(Traits traits, StatusEffect statusEffect)
        {
            this.m_Traits = traits;
            this.m_StatusEffect = statusEffect;
            
            this.Args = new Args(this.m_Traits.gameObject);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Update()
        {
            bool removeAny = false;
            
            for (int i = this.m_List.Count - 1; i >= 0; --i)
            {
                RuntimeStatusEffectData data = this.m_List[i];
                if (data.Update())
                {
                    data.Stop();
                    this.m_List.RemoveAt(i);
                    removeAny = true;
                }
            }

            if (removeAny)
            {
                this.EventRemove?.Invoke(this.m_StatusEffect.ID);
                this.EventChange?.Invoke(this.m_StatusEffect.ID);
            }
        }
        
        public void Add(float timeElapsed)
        {
            int maxStack = this.m_StatusEffect.GetMaxStack(this.Args);
            if (maxStack <= 0) return;

            if (this.Count >= maxStack)
            {
                this.m_List[0].Stop();
                this.m_List.RemoveAt(0);
                this.EventRemove?.Invoke(this.m_StatusEffect.ID);
            }

            this.m_List.Add(new RuntimeStatusEffectData(
                this.m_Traits,
                this.m_StatusEffect,
                timeElapsed
            ));
            
            this.EventAdd?.Invoke(this.m_StatusEffect.ID);
            this.EventChange?.Invoke(this.m_StatusEffect.ID);
        }

        public void Remove(int amount = 1)
        {
            if (this.Count <= 0) return;
            while (this.Count > 0 && amount > 0)
            {
                this.m_List[0].Stop();
                this.m_List.RemoveAt(0);
                
                amount -= 1;
            }

            this.EventRemove?.Invoke(this.m_StatusEffect.ID);
            this.EventChange?.Invoke(this.m_StatusEffect.ID);
        }

        public void RemoveByType(int maskType)
        {
            if (((int) this.m_StatusEffect.Type & maskType) == 0) return;
            for (int i = this.Count - 1; i >= 0; --i)
            {
                this.m_List[i].Stop();
                this.m_List.RemoveAt(i);
            }
            
            this.EventRemove?.Invoke(this.m_StatusEffect.ID);
            this.EventChange?.Invoke(this.m_StatusEffect.ID);
        }

        public RuntimeStatusEffectValue GetValueAt(int index)
        {
            return index < this.m_List.Count 
                ? this.m_List[index].GetValue()
                : default;
        }
    }
}