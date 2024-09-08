using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Time since Last Break")]
    [Description("Returns true if the time since the last broken attack is less than a value")]

    [Category("Melee/Time since Last Break")]
    
    [Parameter("Character", "The Character targeted")]
    [Parameter("Time", "The maximum time for this condition to be true")]

    [Keywords("Combat", "Melee", "Block", "Defend", "Broken", "Destroy")]
    
    [Image(typeof(IconShieldOutline), ColorTheme.Type.Red)]
    
    [Serializable]
    public class ConditionMeleeCompareTimeLastBreak : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();
        [SerializeField] private PropertyGetDecimal m_Time = GetDecimalDecimal.Create(1f);

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"Break Time of {this.m_Character} < {this.m_Time}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return false;

            float currentTime = character.Time.Time;
            float breakTime = character.Combat.LastBreakTime;

            return currentTime - breakTime < this.m_Time.Get(args);
        }
    }
}
