using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Version(0, 1, 1)]
    
    [Title("Add Stat Modifier")]
    [Category("Stats/Add Stat Modifier")]
    
    [Image(typeof(IconStat), ColorTheme.Type.Yellow, typeof(OverlayPlus))]
    [Description("Adds a value Modifier to the selected Stat on a game object's Traits component")]

    [Parameter("Target", "The targeted game object with a Traits component")]
    [Parameter("Stat", "The Stat that removes the Modifier")]
    [Parameter("Type", "If the Modifier changes the Stat by a constant value or by a percentage")]
    [Parameter("Value", "The constant or percentage-based value of the Modifier")]
    
    [Keywords("Slot", "Increase", "Equip", "Fortify")]
    [Keywords("Vitality", "Constitution", "Strength", "Dexterity", "Defense", "Armor")]
    [Keywords("Magic", "Wisdom", "Intelligence")]

    [Serializable]
    public class InstructionStatsAddModifier : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        [SerializeField] private PropertyGetStat m_Stat = new PropertyGetStat();
        [SerializeField] private ModifierType m_Type = ModifierType.Constant;
        [SerializeField] private PropertyGetDecimal m_Value = new PropertyGetDecimal(15f);

        public override string Title => $"Add {this.m_Value} to {this.m_Target}[{this.m_Stat}]";
        
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

            float value = (float) this.m_Value.Get(args);
            runtimeStat.AddModifier(this.m_Type, value);
            
            return DefaultResult;
        }
    }
}