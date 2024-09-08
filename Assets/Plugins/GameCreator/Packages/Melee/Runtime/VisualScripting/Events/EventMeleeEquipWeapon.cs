using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("On Equip Weapon")]
    [Category("Melee/On Equip Weapon")]
    [Description("Executed when the Character equips a new Melee Weapon")]

    [Image(typeof(IconMeleeSword), ColorTheme.Type.Blue)]
    
    [Keywords("Equip", "Unsheathe", "Take", "Sword", "Melee")]

    [Serializable]
    public class EventMeleeEquipWeapon : TEventCharacter
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
            GameObject target = this.m_CachedCharacter.gameObject;
            _ = this.m_Trigger.Execute(target);
        }
    }
}