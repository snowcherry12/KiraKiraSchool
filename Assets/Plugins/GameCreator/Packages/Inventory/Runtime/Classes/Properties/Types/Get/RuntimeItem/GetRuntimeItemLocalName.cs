using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Runtime Item value of a Local Name Variable")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLocalName : PropertyTypeGetRuntimeItem
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueRuntimeItem.TYPE_ID);

        public override RuntimeItem Get(Args args) => this.m_Variable.Get<RuntimeItem>(args);

        public override string String => this.m_Variable.ToString();
    }
}