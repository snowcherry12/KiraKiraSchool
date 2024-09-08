using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Local List Variable from Runtime Item")]
    [Category("Variables/Local List Variable from Runtime Item")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Item value of a Local List Variable with a Runtime Item")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLocalListFromRuntimeItem : PropertyTypeGetItem
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueRuntimeItem.TYPE_ID);

        public override Item Get(Args args) => this.m_Variable.Get<RuntimeItem>(args)?.Item;

        public override string String => this.m_Variable.ToString();
    }
}