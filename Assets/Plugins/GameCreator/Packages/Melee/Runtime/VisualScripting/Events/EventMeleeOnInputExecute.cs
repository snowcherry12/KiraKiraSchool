using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("On Input Execute")]
    [Category("Melee/On Input Execute")]
    [Description("Executed when the Character starts to run the Execute input command")]

    [Parameter("Key", "The key being used as an Execute command")]
    [Image(typeof(IconMeleeInputExecute), ColorTheme.Type.Green)]
    
    [Keywords("Charge", "Input", "Melee", "Attack", "Strike")]

    [Serializable]
    public class EventMeleeOnInputExecute : TEventCharacter
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
            if (stance == null) return;
            
            stance.EventInputExecute += this.OnInputExecute;
        }

        protected override void WhenDisabled(Trigger trigger, Character character)
        {
            this.m_CachedCharacter = character;
            
            MeleeStance stance = character.Combat.RequestStance<MeleeStance>();
            if (stance == null) return;
            
            stance.EventInputExecute -= this.OnInputExecute;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnInputExecute(MeleeKey meleeKey)
        {
            if (meleeKey != this.m_Key) return;
            
            GameObject target = this.m_CachedCharacter.gameObject;
            _ = this.m_Trigger.Execute(target);
        }
    }
}