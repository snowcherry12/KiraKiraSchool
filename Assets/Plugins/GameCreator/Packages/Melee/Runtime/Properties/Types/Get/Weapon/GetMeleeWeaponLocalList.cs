using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Melee Local List Variable")]
    [Category("Variables/Melee Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Melee Weapon value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetMeleeWeaponLocalList : PropertyTypeGetWeapon
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueMeleeWeapon.TYPE_ID);

        public override IWeapon Get(Args args) => this.m_Variable.Get<MeleeWeapon>(args);

        public override string String => this.m_Variable.ToString();
    }
}