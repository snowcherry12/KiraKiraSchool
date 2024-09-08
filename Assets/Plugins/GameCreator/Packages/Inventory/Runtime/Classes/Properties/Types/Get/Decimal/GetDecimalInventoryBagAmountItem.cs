using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Amount of Items")]
    [Category("Inventory/Bag/Amount of Items")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green)]
    [Description("The the number of Item (and sub-types) a Bag contains")]

    [Parameter("Bag", "The targeted Bag component")]
    [Parameter("Item", "The Item type")]
    
    [Keywords("Inventory", "Amount", "Total")]

    [Serializable]
    public class GetDecimalInventoryBagAmountItem : PropertyTypeGetDecimal
    {
        [SerializeField] protected PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        [SerializeField] protected PropertyGetItem m_Item = GetItemInstance.Create();

        public override double Get(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return 0f;

            Item item = this.m_Item.Get(args);
            if (item == null) return 0f;

            return bag.Content.CountType(item);
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryBagAmountItem()
        );

        public override string String => $"Amount of {this.m_Item}";
    }
}