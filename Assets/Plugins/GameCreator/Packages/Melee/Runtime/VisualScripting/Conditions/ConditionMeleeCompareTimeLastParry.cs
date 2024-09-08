using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Time since Last Parry")]
    [Description("Returns true if the time since the last parried attack is less than a value")]

    [Category("Melee/Time since Last Parry")]
    
    [Parameter("Character", "The Character targeted")]
    [Parameter("Time", "The maximum time for this condition to be true")]

    [Keywords("Combat", "Melee", "Block", "Defend")]
    
    [Image(typeof(IconShieldOutline), ColorTheme.Type.Blue, typeof(OverlayDot))]
    
    [Serializable]
    public class ConditionMeleeCompareTimeLastParry : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();
        [SerializeField] private PropertyGetDecimal m_Time = GetDecimalDecimal.Create(1f);

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"Parry Time of {this.m_Character} < {this.m_Time}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return false;

            float currentTime = character.Time.Time;
            float parryTime = character.Combat.LastParryTime;

            return currentTime - parryTime < this.m_Time.Get(args);
        }
    }
}
