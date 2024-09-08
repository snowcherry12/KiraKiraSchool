using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    internal class Modifiers
    {
        [NonSerialized] private ModifierList m_Percentages;
        [NonSerialized] private ModifierList m_Constants;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public Modifiers(int statID)
        {
            this.m_Percentages = new ModifierList(statID);
            this.m_Constants = new ModifierList(statID);
        }

        public Modifiers(int statID, List<Modifier> percentages, List<Modifier> constants)
        {
            this.m_Percentages = new ModifierList(statID, percentages);
            this.m_Constants = new ModifierList(statID, constants);
        }
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public int Count => this.m_Percentages.Count + this.m_Constants.Count;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public double Calculate(double value)
        {
            value *= 1f + this.m_Percentages.Value;
            value += this.m_Constants.Value;

            return value;
        }

        // INTERNAL METHODS: ----------------------------------------------------------------------
        
        internal void AddPercentage(double percent)
        {
            this.m_Percentages.Add(percent);
        }
        
        internal void AddConstant(double value)
        {
            this.m_Constants.Add(value);
        }
        
        internal bool RemovePercentage(double percent)
        {
            return this.m_Percentages.Remove(percent);
        }
        
        internal bool RemoveConstant(double value)
        {
            return this.m_Constants.Remove(value);
        }

        internal void Clear()
        {
            this.m_Percentages.Clear();
            this.m_Constants.Clear();
        }
    }
}