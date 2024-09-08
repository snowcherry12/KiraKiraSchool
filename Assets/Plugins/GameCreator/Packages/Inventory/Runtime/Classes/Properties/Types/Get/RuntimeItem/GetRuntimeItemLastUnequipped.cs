using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Unequipped")]
    [Category("Equipment/Last Unequipped")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Red)]
    [Description("A reference to the last Runtime Item unequipped")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLastUnequipped : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => RuntimeItem.Bag_LastItemUnequipped;
        public override RuntimeItem Get(GameObject gameObject) => RuntimeItem.Bag_LastItemUnequipped;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastUnequipped instance = new GetRuntimeItemLastUnequipped();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Unequipped]";
    }
}