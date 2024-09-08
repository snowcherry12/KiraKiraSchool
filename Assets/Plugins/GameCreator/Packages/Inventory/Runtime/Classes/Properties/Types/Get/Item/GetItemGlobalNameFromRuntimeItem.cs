using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Global Name Variable from Runtime Item")]
    [Category("Variables/Global Name Variable from Runtime Item")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Item value of a Global Name Variable with a Runtime Item")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemGlobalNameFromRuntimeItem : PropertyTypeGetItem
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueRuntimeItem.TYPE_ID);

        public override Item Get(Args args) => this.m_Variable.Get<RuntimeItem>(args)?.Item;

        public override string String => this.m_Variable.ToString();
    }
}