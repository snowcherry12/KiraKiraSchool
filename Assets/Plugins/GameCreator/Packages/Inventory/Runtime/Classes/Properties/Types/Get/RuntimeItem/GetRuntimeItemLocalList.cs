using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Runtime Item value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLocalList : PropertyTypeGetRuntimeItem
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueRuntimeItem.TYPE_ID);

        public override RuntimeItem Get(Args args) => this.m_Variable.Get<RuntimeItem>(args);

        public override string String => this.m_Variable.ToString();
    }
}