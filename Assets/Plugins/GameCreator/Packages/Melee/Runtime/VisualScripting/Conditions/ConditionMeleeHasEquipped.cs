using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Has Equipped Melee")]
    [Description("Returns true if the Character has a specific Melee Weapon equipped")]

    [Category("Melee/Has Equipped Melee")]
    
    [Parameter("Character", "The targeted Character")]
    [Parameter("Weapon", "The Melee Weapon to check if it is equipped")]

    [Keywords("Combat", "Melee")]
    
    [Image(typeof(IconMeleeSword), ColorTheme.Type.Blue)]
    [Serializable]
    public class ConditionMeleeHasEquipped : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetWeapon m_Weapon = new PropertyGetWeapon();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"has {this.m_Character} Equipped {this.m_Weapon}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            MeleeWeapon meleeWeapon = this.m_Weapon.Get(args) as MeleeWeapon;
            
            return character != null && 
                   meleeWeapon != null &&
                   character.Combat.IsEquipped(meleeWeapon);
        }
    }
}
