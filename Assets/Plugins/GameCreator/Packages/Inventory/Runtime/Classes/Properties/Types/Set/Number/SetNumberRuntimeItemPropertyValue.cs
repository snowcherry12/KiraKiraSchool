using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Runtime Item Property Value")]
    [Category("Inventory/Runtime Item Property Value")]
    
    [Image(typeof(IconProperty), ColorTheme.Type.Blue)]
    [Description("Sets the property Value of the Runtime Item")]
    
    [Serializable]
    public class SetNumberRuntimeItemPropertyValue : PropertyTypeSetNumber
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectInventoryBag.Create();
        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        public override double Get(Args args) => this.GetValue(args);

        public override void Set(double value, Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return;
            
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null) return;

            if (runtimeItem.Properties.TryGetValue(this.m_PropertyId, out RuntimeProperty prop))
            {
                prop.Number = (int) value;
            }
        }

        private double GetValue(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return 0;
            
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null) return 0;
            
            return runtimeItem.Properties.TryGetValue(this.m_PropertyId, out RuntimeProperty prop)
                ? prop.Number
                : 0;
        }

        public override string String => $"{this.m_RuntimeItem}[{this.m_PropertyId}] Value";
    }
}