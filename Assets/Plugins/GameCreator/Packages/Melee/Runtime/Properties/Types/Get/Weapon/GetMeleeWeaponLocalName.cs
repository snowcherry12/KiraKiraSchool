using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Melee Local Name Variable")]
    [Category("Variables/Melee Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Melee Weapon value of a Local Name Variable")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetMeleeWeaponLocalName : PropertyTypeGetWeapon
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueMeleeWeapon.TYPE_ID);

        public override IWeapon Get(Args args) => this.m_Variable.Get<MeleeWeapon>(args);

        public override string String => this.m_Variable.ToString();
    }
}