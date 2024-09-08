using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Shield value of a Global Name Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetShieldGlobalName : PropertyTypeGetShield
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueShield.TYPE_ID);

        public override IShield Get(Args args) => this.m_Variable.Get<Shield>(args);

        public override string String => this.m_Variable.ToString();
    }
}