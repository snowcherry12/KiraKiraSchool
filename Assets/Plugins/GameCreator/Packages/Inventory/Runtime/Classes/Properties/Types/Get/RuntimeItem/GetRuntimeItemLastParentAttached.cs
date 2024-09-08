using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Parent Attached")]
    [Category("Sockets/Last Parent Attached")]
    
    [Image(typeof(IconSocket), ColorTheme.Type.Blue, typeof(OverlayPlus))]
    [Description("A reference to the last Item that one of its Sockets was attached")]
    
    [Keywords("Socket", "Embed", "Enchant", "Add")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLastParentAttached : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => RuntimeItem.Socket_LastParentAttached;
        public override RuntimeItem Get(GameObject gameObject) => RuntimeItem.Socket_LastParentAttached;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastParentAttached instance = new GetRuntimeItemLastParentAttached();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Parent Attached]";
    }
}