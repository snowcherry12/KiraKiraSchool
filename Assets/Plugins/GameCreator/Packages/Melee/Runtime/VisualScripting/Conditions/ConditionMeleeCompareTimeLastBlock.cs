using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Time since Last Block")]
    [Description("Returns true if the time since the last blocked attack is less than a value")]

    [Category("Melee/Time since Last Block")]
    
    [Parameter("Character", "The Character targeted")]
    [Parameter("Time", "The maximum time for this condition to be true")]

    [Keywords("Combat", "Melee", "Block", "Defend")]
    
    [Image(typeof(IconShieldOutline), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionMeleeCompareTimeLastBlock : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectSelf.Create();
        [SerializeField] private PropertyGetDecimal m_Time = GetDecimalDecimal.Create(1f);

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"Block Time of {this.m_Character} < {this.m_Time}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return false;

            float currentTime = character.Time.Time;
            float blockTime = character.Combat.LastBlockTime;

            return currentTime - blockTime < this.m_Time.Get(args);
        }
    }
}
