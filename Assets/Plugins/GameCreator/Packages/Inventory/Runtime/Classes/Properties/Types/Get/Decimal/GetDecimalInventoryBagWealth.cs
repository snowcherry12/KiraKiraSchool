using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Bag Wealth")]
    [Category("Inventory/Weight/Bag Wealth")]
    
    [Image(typeof(IconCurrency), ColorTheme.Type.Yellow)]
    [Description("The the current Wealth of a Bag component")]

    [Parameter("Bag", "The targeted Bag component")]
    [Parameter("Currency", "The currency of the Bag")]
    
    [Keywords("Money", "Currency", "Cash", "Coin", "Inventory")]

    [Serializable]
    public class GetDecimalInventoryBagWealth : PropertyTypeGetDecimal
    {
        [SerializeField] protected PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        [SerializeField] protected Currency m_Currency;

        public override double Get(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return 0f;

            return bag.Wealth.Get(this.m_Currency);
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryBagWealth()
        );

        public override string String => $"{this.m_Bag} Wealth";
    }
}