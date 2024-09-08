using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Inventory
{
    [Title("Local List Variable")]
    [Category("Local List Variable")]
    
    [Description("Sets the Runtime Item value on a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable] [HideLabelsInEditor]
    public class SetRuntimeItemLocalList : PropertyTypeSetRuntimeItem
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueRuntimeItem.TYPE_ID);

        public override void Set(RuntimeItem value, Args args) => this.m_Variable.Set(value, args);
        public override RuntimeItem Get(Args args) => this.m_Variable.Get(args) as RuntimeItem;

        public static PropertySetRuntimeItem Create => new PropertySetRuntimeItem(
            new SetRuntimeItemLocalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}