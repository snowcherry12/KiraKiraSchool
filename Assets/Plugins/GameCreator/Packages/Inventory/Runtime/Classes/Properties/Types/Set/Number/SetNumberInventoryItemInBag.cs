using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Items in Bag")]
    [Category("Inventory/Items in Bag")]
    
    [Image(typeof(IconBagSolid), ColorTheme.Type.Green)]
    [Description("Sets the amount of a specified Item in the specified Bag")]
    
    [Serializable]
    public class SetNumberInventoryItemInBag : PropertyTypeSetNumber
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();

        public override double Get(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            Item item = this.m_Item.Get(args);

            return bag != null && item != null 
                ? bag.Content.CountType(item)
                : 0;
        }

        public override void Set(double value, Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            Item item = this.m_Item.Get(args);
            
            if (bag == null) return;
            if (item == null) return;

            int nextCount = (int) value;
            int currentCount = bag.Content.CountType(item);

            if (currentCount > nextCount)
            {
                int numToRemove = currentCount - nextCount;
                for (int i = 0; i < numToRemove; ++i)
                {
                    RuntimeItem removeRuntimeItem = bag.Content.FindRuntimeItem(item);
                    bag.Content.Remove(removeRuntimeItem);
                }
            }

            if (currentCount < nextCount)
            {
                int numToAdd = nextCount - currentCount;
                for (int i = 0; i < numToAdd; ++i)
                {
                    bag.Content.AddType(item, true);
                }
            }
        }

        public override string String => $"{this.m_Item} in {this.m_Bag}";
    }
}