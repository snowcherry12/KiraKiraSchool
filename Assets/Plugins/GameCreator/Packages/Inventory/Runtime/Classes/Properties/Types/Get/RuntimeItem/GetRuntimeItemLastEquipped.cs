using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Equipped")]
    [Category("Equipment/Last Equipped")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Blue)]
    [Description("A reference to the last Runtime Item equipped")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLastEquipped : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => RuntimeItem.Bag_LastItemEquipped;
        public override RuntimeItem Get(GameObject gameObject) => RuntimeItem.Bag_LastItemEquipped;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastEquipped instance = new GetRuntimeItemLastEquipped();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Last Equipped]";
    }
}