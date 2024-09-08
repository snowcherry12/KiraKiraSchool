using System;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Shield value of a Local Name Variable")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetShieldLocalName : PropertyTypeGetShield
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueShield.TYPE_ID);

        public override IShield Get(Args args) => this.m_Variable.Get<Shield>(args);

        public override string String => this.m_Variable.ToString();
    }
}