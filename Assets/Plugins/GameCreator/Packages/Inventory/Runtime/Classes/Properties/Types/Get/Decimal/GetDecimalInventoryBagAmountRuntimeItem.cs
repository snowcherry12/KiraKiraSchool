using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Amount of Runtime Items")]
    [Category("Inventory/Bag/Amount of Runtime Items")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Blue)]
    [Description("The the number of Item's Runtime Item (and sub-types) a Bag contains")]

    [Parameter("Bag", "The targeted Bag component")]
    [Parameter("Runtime Item", "The Item of the Runtime Item type")]
    
    [Keywords("Inventory", "Amount", "Total")]

    [Serializable]
    public class GetDecimalInventoryBagAmountRuntimeItem : PropertyTypeGetDecimal
    {
        [SerializeField] protected PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        [SerializeField] protected PropertyGetRuntimeItem m_RuntimeItem = GetRuntimeItemLastAdded.Create();

        public override double Get(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return 0f;

            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null || runtimeItem.Item) return 0f;

            return bag.Content.CountType(runtimeItem.Item);
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryBagAmountItem()
        );

        public override string String => $"Amount of {this.m_RuntimeItem}";
    }
}