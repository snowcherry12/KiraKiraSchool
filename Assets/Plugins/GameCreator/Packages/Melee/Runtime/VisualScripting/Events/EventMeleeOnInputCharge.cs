using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("On Input Charge")]
    [Category("Melee/On Input Charge")]
    [Description("Executed when the Character starts to run a Charge input command")]

    [Parameter("Key", "The key being used as a Charge command")]
    [Image(typeof(IconMeleeInputCharge), ColorTheme.Type.Yellow)]
    
    [Keywords("Charge", "Input", "Melee", "Execute", "Hold", "Load")]

    [Serializable]
    public class EventMeleeOnInputCharge : TEventCharacter
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private MeleeKey m_Key = MeleeKey.A;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Character m_CachedCharacter;

        // METHODS: -------------------------------------------------------------------------------
        
        protected override void WhenEnabled(Trigger trigger, Character character)
        {
            this.m_CachedCharacter = character;
            
            MeleeStance stance = character.Combat.RequestStance<MeleeStance>();
            if (stance.Character == null) return;
            
            stance.EventInputCharge += this.OnInputCharge;
        }

        protected override void WhenDisabled(Trigger trigger, Character character)
        {
            this.m_CachedCharacter = character;
            
            MeleeStance stance = character.Combat.RequestStance<MeleeStance>();
            if (stance.Character == null) return;
            
            stance.EventInputCharge -= this.OnInputCharge;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnInputCharge(MeleeKey meleeKey)
        {
            if (meleeKey == this.m_Key) return;
            
            GameObject target = this.m_CachedCharacter.gameObject;
            _ = this.m_Trigger.Execute(target);
        }
    }
}