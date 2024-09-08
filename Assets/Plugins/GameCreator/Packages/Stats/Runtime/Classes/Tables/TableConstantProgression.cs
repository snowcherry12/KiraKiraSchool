using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Image(typeof(IconTable), ColorTheme.Type.Blue)]
    [Title("Constant Progression")]
    [Category("Constant Progression")]
    
    [Description("Each level requires the same amount of experience to reach the next one")]

    [Serializable]
    public class TableConstantProgression : TTable
    {
        // +--------------------------------------------------------------------------------------+
        // | EXP_Level(n + 1) = experience                                                        |
        // |                                                                                      |
        // | n: is the current level.                                                             |
        // | experience: is the amount of experience required per level.                          |
        // +--------------------------------------------------------------------------------------+
        
        [SerializeField] private int m_MaxLevel = 99;
        [SerializeField] private int m_Increment = 50;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override int MinLevel => 1;
        public override int MaxLevel => this.m_MaxLevel;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public TableConstantProgression() : base()
        { }

        public TableConstantProgression(int maxLevel, int increment) : this()
        {
            this.m_MaxLevel = maxLevel;
            this.m_Increment = increment;
        }
        
        // IMPLEMENT METHODS: ---------------------------------------------------------------------

        protected override int LevelFromCumulative(int cumulative)
        {
            float level = (float) (cumulative + this.m_Increment) / this.m_Increment;
            return Mathf.Clamp(Mathf.FloorToInt(level), this.MinLevel, this.MaxLevel + 1);
        }

        protected override int CumulativeFromLevel(int level)
        {
            return level * this.m_Increment - this.m_Increment;
        }
    }
}