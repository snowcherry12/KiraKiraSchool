using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    public class State
    {
        [NonSerialized] private readonly HashSet<PropertyName> m_Beliefs;

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public State()
        {
            this.m_Beliefs = new HashSet<PropertyName>();
        }

        private State(State state) : this()
        {
            foreach (PropertyName value in state.m_Beliefs)
            {
                this.m_Beliefs.Add(value);
            }
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Get(string name)
        {
            return this.m_Beliefs.Contains(name);
        }

        public bool CanResolveWith(Beliefs beliefs)
        {
            if (beliefs == null) return true;
            foreach (Belief belief in beliefs.List)
            {
                switch (belief.Value)
                {
                    case true when this.Get(belief.Name.String):
                    case false when !this.Get(belief.Name.String):
                        continue;
                    
                    default: return false;
                }
            }

            return true;
        }

        public bool CanResolveWith(State state)
        {
            if (state == null) return true;

            foreach (PropertyName belief in state.m_Beliefs)
            {
                if (this.m_Beliefs.Contains(belief)) continue;
                return false;
            }

            return true;
        }

        public void Set(string name, bool value)
        {
            if (value) this.m_Beliefs.Add(name);
            else this.m_Beliefs.Remove(name);
        }

        public State Copy()
        {
            return new State(this);
        }
        
        public void Clear()
        {
            this.m_Beliefs.Clear();
        }

        public void Apply(Beliefs beliefs)
        {
            if (beliefs == null) return;
            foreach (Belief belief in beliefs.List)
            {
                this.Set(belief.Name.String, belief.Value);
            }
        }

        public void Reset(Beliefs beliefs)
        {
            this.Clear();
            this.Apply(beliefs);
        }
    }
}