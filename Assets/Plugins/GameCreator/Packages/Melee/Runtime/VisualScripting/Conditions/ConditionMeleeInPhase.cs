using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("In Attack Phase")]
    [Description("Returns true if the character is in any of the specified attack phases")]

    [Category("Melee/In Attack Phase")]
    
    [Parameter("Character", "The targeted Character")]
    [Parameter("Phases", "The attack phases the character might be in")]

    [Keywords("Combat", "Melee", "Attack", "Anticipation", "Strike", "Activation", "Recovery")]
    
    [Image(typeof(IconMeleeSword), ColorTheme.Type.Green)]
    [Serializable]
    public class ConditionMeleeInPhase : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] private MeleePhaseMask m_Phases = MeleePhaseMask.None;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Character} is {this.m_Phases}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return false;

            MeleeStance melee = character.Combat.RequestStance<MeleeStance>();
            return ((int) melee.CurrentPhase & (int) this.m_Phases) > 0;
        }
    }
}
