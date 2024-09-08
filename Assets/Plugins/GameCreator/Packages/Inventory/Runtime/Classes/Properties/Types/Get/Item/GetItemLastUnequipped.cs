using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Unequipped")]
    [Category("Equipment/Last Unequipped")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Red)]
    [Description("A reference to the last Item unequipped")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastUnequipped : PropertyTypeGetItem
    {
        public override Item Get(Args args) => RuntimeItem.Bag_LastItemUnequipped?.Item;
        public override Item Get(GameObject gameObject) => RuntimeItem.Bag_LastItemUnequipped?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastUnequipped instance = new GetItemLastUnequipped();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Last Unequipped]";
    }
}