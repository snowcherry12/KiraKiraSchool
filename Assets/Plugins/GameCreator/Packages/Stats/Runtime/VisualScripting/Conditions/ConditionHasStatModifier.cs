using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Has Stat Modifiers")]
    [Category("Stats/Has Stat Modifiers")]
    
    [Image(typeof(IconStat), ColorTheme.Type.Yellow)]
    [Description("Returns true if the targeted Stat component has a Stat Modifier")]

    [Parameter("Target", "The targeted game object with a Traits component")]
    [Parameter("Stat", "The Stat that checks if it has a Stat Modifier")]

    [Keywords("Skill", "Throw", "Check", "Dice")]
    [Keywords("Lock", "Pick", "Charisma", "Speech")]

    [Serializable]
    public class ConditionHasStatModifiers : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetStat m_Stat = new PropertyGetStat();

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Summary => $"has {this.m_Target} Modifier {this.m_Stat}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Stat stat = this.m_Stat.Get(args);
            if (stat == null) return false;
            
            Traits target = this.m_Target.Get<Traits>(args);
            return target != null && target.RuntimeStats.Get(stat.ID).HasModifiers;
        }
    }
}
