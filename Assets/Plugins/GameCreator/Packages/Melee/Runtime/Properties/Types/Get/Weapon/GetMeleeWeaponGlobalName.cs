using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Melee Global Name Variable")]
    [Category("Variables/Melee Global Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Melee Weapon value of a Global Name Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetMeleeWeaponGlobalName : PropertyTypeGetWeapon
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueMeleeWeapon.TYPE_ID);

        public override IWeapon Get(Args args) => this.m_Variable.Get<MeleeWeapon>(args);

        public override string String => this.m_Variable.ToString();
    }
}