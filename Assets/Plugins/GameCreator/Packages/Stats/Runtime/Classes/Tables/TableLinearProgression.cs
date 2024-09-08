using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Image(typeof(IconTable), ColorTheme.Type.Green)]
    [Title("Linear Progression")]
    [Category("Linear Progression")]
    
    [Description(
        "The experience needed to reach the next level is equal to the previous level " +
        "multiplied by a constant increment"
    )]

    [Serializable]
    public class TableLinearProgression : TTable
    {
        // +--------------------------------------------------------------------------------------+
        // | EXP_Level(n + 1) = EXP_Level(n) + (n * increment)                                    |
        // |                                                                                      |
        // | n: is the current level.                                                             |
        // | increment: is the amount of experience added per level.                              |
        // +--------------------------------------------------------------------------------------+
        
        [SerializeField] private int m_MaxLevel = 99;
        [SerializeField] private int m_IncrementPerLevel = 50;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override int MinLevel => 1;
        public override int MaxLevel => this.m_MaxLevel;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public TableLinearProgression() : base()
        { }

        public TableLinearProgression(int maxLevel, int incrementPerLevel) : this()
        {
            this.m_MaxLevel = maxLevel;
            this.m_IncrementPerLevel = incrementPerLevel;
        }
        
        // IMPLEMENT METHODS: ---------------------------------------------------------------------

        protected override int LevelFromCumulative(int cumulative)
        {
            float squareRoot = Mathf.Sqrt(1f + 8f * cumulative / this.m_IncrementPerLevel);
            float level = (1 + squareRoot) / 2.0f;
            
            return Mathf.Clamp(Mathf.FloorToInt(level), this.MinLevel, this.MaxLevel + 1);
        }

        protected override int CumulativeFromLevel(int level)
        {
            float power = Mathf.Pow(level, 2.0f);
            float result = (power - level) * this.m_IncrementPerLevel / 2.0f;
            
            return Mathf.FloorToInt(result);
        }
    }
}