using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class Thoughts : TPolymorphicList<Thought>
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeReference] private Thought[] m_List = Array.Empty<Thought>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override int Length => this.m_List.Length;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        // public void ResetTo(State state, Processor processor)
        // {
        //     if (state == null) return;
        //     state.Clear();
        //     
        //     foreach (Thought thought in this.m_List)
        //     {
        //         if (string.IsNullOrEmpty(thought.Name)) continue;
        //         
        //         bool value = thought.GetValue(processor.Args);
        //         state.Set(thought.Name, value);
        //     }
        // }

        public State Meditate(Processor processor)
        {
            State state = new State();
            
            foreach (Thought thought in this.m_List)
            {
                if (string.IsNullOrEmpty(thought.Name)) continue;
                
                bool value = thought.GetValue(processor.Args);
                state.Set(thought.Name, value);
            }

            return state;
        }
    }
}