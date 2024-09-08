using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("Last Cancel Successful")]
    [Description("Returns true if the last attempt to cancel a skill was successful")]

    [Category("Melee/Last Cancel Successful")]
    
    [Parameter("Character", "The Character that might have attempted to cancel its skill")]

    [Keywords("Combat", "Melee", "Attack")]
    
    [Image(typeof(IconMeleeSkill), ColorTheme.Type.Red, typeof(OverlayCross))]
    [Serializable]
    public class ConditionMeleeWasCanceled : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"last Cancel on {this.m_Character} was Successful";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return false;
            
            return character.Combat
                .RequestStance<MeleeStance>()
                .LastCancelSuccessful;
        }
    }
}
