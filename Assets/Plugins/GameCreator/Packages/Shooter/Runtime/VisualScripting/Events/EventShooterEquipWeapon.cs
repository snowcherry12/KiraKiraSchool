using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Title("On Equip Weapon")]
    [Category("Shooter/On Equip Weapon")]
    [Description("Executed when the Character equips a new Shooter Weapon")]

    [Image(typeof(IconPistol), ColorTheme.Type.Blue)]
    
    [Keywords("Equip", "Unsheathe", "Take", "Sword", "Shooter")]

    [Serializable]
    public class EventShooterEquipWeapon : TEventCharacter
    {
        [NonSerialized] private Character m_CachedCharacter;
        
        // METHODS: -------------------------------------------------------------------------------
        
        protected override void WhenEnabled(Trigger trigger, Character character)
        {
            this.m_CachedCharacter = character;
            character.Combat.EventEquip += this.OnEquip;
        }

        protected override void WhenDisabled(Trigger trigger, Character character)
        {
            this.m_CachedCharacter = character;
            character.Combat.EventEquip -= this.OnEquip;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnEquip(IWeapon weapon, GameObject gameObject)
        {
            if (weapon is not ShooterWeapon) return;
            
            GameObject target = this.m_CachedCharacter.gameObject;
            _ = this.m_Trigger.Execute(target);
        }
    }
}