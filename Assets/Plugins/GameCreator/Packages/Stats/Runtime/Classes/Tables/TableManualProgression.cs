using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Image(typeof(IconTable), ColorTheme.Type.Yellow)]
    [Title("Manual Progression")]
    [Category("Manual Progression")]
    
    [Description("Manually defines the amount of experience needed to reach the next level")]

    [Serializable]
    public class TableManualProgression : TTable
    {
        // +--------------------------------------------------------------------------------------+
        // | EXP_Level(n) = X(n)                                                                  |
        // |                                                                                      |
        // | n: is the current level.                                                             |
        // | X(n): is the n-th variable from the experience table                                 |
        // +--------------------------------------------------------------------------------------+
        
        [SerializeField] private int[] m_Increments = new int[99];
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override int MinLevel => 1;
        public override int MaxLevel => this.m_Increments.Length - 1;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public TableManualProgression() : base()
        {
            for (int i = 0; i < this.m_Increments.Length; ++i)
            {
                this.m_Increments[i] = 10 + 5 * i;
            }
        }

        public TableManualProgression(int[] increments) : this()
        {
            this.m_Increments = increments;
        }
        
        // IMPLEMENT METHODS: ---------------------------------------------------------------------

        protected override int LevelFromCumulative(int cumulative)
        {
            int sum = 0;
            
            for (int i = 0; i < this.m_Increments.Length; ++i)
            {
                if (cumulative < sum) return i;
                sum += this.m_Increments[i];
            }

            return this.MaxLevel;
        }

        protected override int CumulativeFromLevel(int level)
        {
            int sum = 0;
            for (int i = 0; i < level - 1; ++i)
            {
                sum += this.m_Increments[i];
            }

            return sum;
        }
    }
}