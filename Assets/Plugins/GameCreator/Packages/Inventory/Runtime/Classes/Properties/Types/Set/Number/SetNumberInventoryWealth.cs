using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Bag Wealth")]
    [Category("Inventory/Bag Wealth")]
    
    [Image(typeof(IconCurrency), ColorTheme.Type.Yellow)]
    [Description("Sets the currency of a Bag")]
    
    [Serializable]
    public class SetNumberInventoryWealth : PropertyTypeSetNumber
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectInventoryBag.Create();
        [SerializeField] private Currency m_Currency;

        public override double Get(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            return bag != null ? bag.Wealth.Get(this.m_Currency) : 0;
        }

        public override void Set(double value, Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return;

            bag.Wealth.Set(this.m_Currency, (int) value);
        }

        public override string String => $"{this.m_Bag} Wealth";
    }
}