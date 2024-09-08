using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Local Name Variable from Runtime Item")]
    [Category("Variables/Local Name Variable from Runtime Item")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Item value of a Local Name Variable with a Runtime Item")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetItemLocalNameFromRuntimeItem : PropertyTypeGetItem
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueRuntimeItem.TYPE_ID);

        public override Item Get(Args args) => this.m_Variable.Get<RuntimeItem>(args)?.Item;

        public override string String => this.m_Variable.ToString();
    }
}