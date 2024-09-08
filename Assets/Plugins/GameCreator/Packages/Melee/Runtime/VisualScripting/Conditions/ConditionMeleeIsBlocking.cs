using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Is Blocking")]
    [Description("Returns true if the specified Character is blocking attacks")]

    [Category("Melee/Is Blocking")]
    
    [Parameter("Character", "The Character that might be blocking attacks")]

    [Keywords("Combat", "Melee", "Block", "Defend")]
    
    [Image(typeof(IconShieldSolid), ColorTheme.Type.Green)]
    [Serializable]
    public class ConditionMeleeIsBlocking : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Character} Blocking";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            return character != null && character.Combat.Block.IsBlocking;
        }
    }
}
