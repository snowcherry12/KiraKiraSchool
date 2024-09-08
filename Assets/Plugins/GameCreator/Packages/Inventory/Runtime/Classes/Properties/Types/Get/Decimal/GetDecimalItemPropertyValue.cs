using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Item Property Value")]
    [Category("Inventory/Item Property Value")]
    
    [Image(typeof(IconProperty), ColorTheme.Type.Green)]
    [Description("Returns the property Value of the Item")]
    
    [Serializable]
    public class GetDecimalItemPropertyValue : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectInventoryBag.Create();
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        public override double Get(Args args) => this.GetValue(args);

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