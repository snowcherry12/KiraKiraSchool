using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Compare Stat")]
    [Description("Returns true if the Stat comparison is successful")]

    [Category("Stats/Compare Stat")]
    
    [Parameter("Traits", "The targeted game object with a Traits component")]
    [Parameter("Stat", "The Stat type value that is compared")]
    [Parameter("Comparison", "The comparison operation performed between both values")]
    [Parameter("Compare To", "The decimal value that is compared against")]
    
    [Keywords("Vitality", "Constitution", "Strength", "Dexterity", "Defense", "Armor")]
    [Keywords("Magic", "Wisdom", "Intelligence")]
    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    
    [Serializable]
    public class ConditionCompareStat : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetStat m_Stat = new PropertyGetStat();

        [SerializeField] 
        private CompareDouble m_CompareTo = new CompareDouble();

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Summary => $"{this.m_Traits}[{this.m_Stat}] {this.m_CompareTo}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            GameObject target = this.m_Traits.Get(args);
            if (target == null) return false;

            Traits traits = target.Get<Traits>();
            if (traits == null) return false;

            Stat stat = this.m_Stat.Get(args);
            if (stat == null) return false;
            
            double value = traits.RuntimeStats.Get(stat.ID).Value;
            return this.m_CompareTo.Match(value, args);
        }
    }
}
