using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Image(typeof(IconTable), ColorTheme.Type.Red)]
    [Title("Geometric Progression")]
    [Category("Geometric Progression")]
    
    [Description(
        "The experience in each level after the first is calculated by multiplying the previous " +
        "one by a fixed coefficient rate"
    )]

    [Serializable]
    public class TableGeometricProgression : TTable
    {
        private const float ZERO = 0.0001f;
        
        // +--------------------------------------------------------------------------------------+
        // | EXP_Level(n + 1) = EXP_Level(n) * rate                                               |
        // |                                                                                      |
        // | n: is the current level.                                                             |
        // | rate: the incremental ratio of experience from the previous level                    |
        // +--------------------------------------------------------------------------------------+
        
        [SerializeField] private int m_MaxLevel = 99;
        [SerializeField] private int m_Increment = 50;
        [SerializeField] private float m_Rate = 1.05f;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override int MinLevel => 1;
        public override int MaxLevel => this.m_MaxLevel;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public TableGeometricProgression() : base()
        { }

        public TableGeometricProgression(int maxLevel, int increment, float rate) : this()
        {
            this.m_MaxLevel = maxLevel;
            this.m_Increment = increment;
            this.m_Rate = rate;
        }
        
        // IMPLEMENT METHODS: ---------------------------------------------------------------------

        protected override int LevelFromCumulative(int cumulative)
        {
            float value = ((float) cumulative + this.m_Increment + 1f) * (this.m_Rate - 1f);
            float result = Mathf.Log(value / this.m_Increment + 1f, this.m_Rate);
            
            return Mathf.FloorToInt(result);
        }

        protected override int CumulativeFromLevel(int level)
        {
            float value = (Mathf.Pow(this.m_Rate, level) - 1f) / (this.m_Rate - 1f);
            return Mathf.FloorToInt(this.m_Increment * value) - this.m_Increment;
        }
    }
}