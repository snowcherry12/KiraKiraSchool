using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("On Unequip Weapon")]
    [Category("Shooter/On Unequip Weapon")]
    [Description("Executed when the Character removes a new Shooter Weapon")]

    [Image(typeof(IconPistol), ColorTheme.Type.Red)]
    
    [Keywords("Unequip", "sheathe", "Take", "Sword", "Shooter")]

    [Serializable]
    public class EventShooterUnequipWeapon : TEventCharacter
    {
        [NonSerialized] private Character m_CachedCharacter;
        
        // METHODS: -------------------------------------------------------------------------------
        
        protected override void WhenEnabled(Trigger trigger, Character character)
        {
            this.m_CachedCharacter = character;
            character.Combat.EventUnequip += this.OnUnequip;
        }

        protected override void WhenDisabled(Trigger trigger, Character character)
        {
            this.m_CachedCharacter = character;
            character.Combat.EventUnequip -= this.OnUnequip;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnUnequip(IWeapon weapon, GameObject gameObject)
        {
            if (weapon is not ShooterWeapon) return;
            
            GameObject target = this.m_CachedCharacter.gameObject;
            _ = this.m_Trigger.Execute(target);
        }
    }
}