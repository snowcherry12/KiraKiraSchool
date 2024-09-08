using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Equipped")]
    [Category("Equipment/Last Equipped")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Blue)]
    [Description("A reference to the last Item equipped")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastEquipped : PropertyTypeGetItem
    {
        public override Item Get(Args args) => RuntimeItem.Bag_LastItemEquipped?.Item;
        public override Item Get(GameObject gameObject) => RuntimeItem.Bag_LastItemEquipped?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastEquipped instance = new GetItemLastEquipped();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Equipped]";
    }
}