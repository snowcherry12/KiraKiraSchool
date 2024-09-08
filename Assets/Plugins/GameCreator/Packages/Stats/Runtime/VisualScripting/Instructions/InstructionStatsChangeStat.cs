using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Version(0, 1, 1)]
    
    [Title("Change Stat")]
    [Category("Stats/Change Stat")]
    
    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    [Description("Changes the base Stat value of a game object's Traits component")]

    [Parameter("Target", "The targeted game object with a Traits component")]
    [Parameter("Stat", "The Stat type that changes its value")]
    [Parameter("Change", "The value changed")]
    
    [Keywords("Vitality", "Constitution", "Strength", "Dexterity", "Defense", "Armor")]
    [Keywords("Magic", "Wisdom", "Intelligence")]
    
    [Serializable]
    public class InstructionStatsChangeStat : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        [SerializeField] private PropertyGetStat m_Stat = new PropertyGetStat();
        [SerializeField] private ChangeDecimal m_Change = new ChangeDecimal(100f);
        
        public override string Title => $"{this.m_Target}[{this.m_Stat}] {this.m_Change}";
        
        protected override Task Run(Args args)
        {
            GameObject target = this.m_Target.Get(args);
            if (target == null) return DefaultResult;

            Traits traits = target.Get<Traits>();
            if (traits == null) return DefaultResult;

            Stat stat = this.m_Stat.Get(args);
            if (stat == null) return DefaultResult;
            
            RuntimeStatData runtimeStat = traits.RuntimeStats.Get(stat.ID);
            if (runtimeStat == null) return DefaultResult;

            runtimeStat.Base = (float) this.m_Change.Get(runtimeStat.Base, args);
            return DefaultResult;
        }
    }
}