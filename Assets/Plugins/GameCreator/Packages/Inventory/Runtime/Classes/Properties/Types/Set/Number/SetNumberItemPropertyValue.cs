using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Item Property Value")]
    [Category("Inventory/Item Property Value")]
    
    [Image(typeof(IconProperty), ColorTheme.Type.Green)]
    [Description("Sets the property Value of the Item")]
    
    [Serializable]
    public class SetNumberItemPropertyValue : PropertyTypeSetNumber
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectInventoryBag.Create();
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        public override double Get(Args args) => this.GetValue(args);

        public override void Set(double value, Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return;
            
            Item item = this.m_Item.Get(args);
            if (item == null) return;

            RuntimeItem runtimeItem = bag.Content.FindRuntimeItem(item);
            
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
            
            Item item = this.m_Item.Get(args);
            if (item == null) return 0;

            RuntimeItem runtimeItem = bag.Content.FindRuntimeItem(item);
            if (runtimeItem == null) return 0;
            
            return runtimeItem.Properties.TryGetValue(this.m_PropertyId, out RuntimeProperty prop)
                ? prop.Number
                : 0;
        }

        public override string String => $"{this.m_Item}[{this.m_PropertyId}] Value";
    }
}