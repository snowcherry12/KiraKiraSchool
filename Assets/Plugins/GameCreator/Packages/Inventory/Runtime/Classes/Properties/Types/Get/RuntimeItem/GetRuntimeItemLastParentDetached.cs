using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Parent Detached")]
    [Category("Sockets/Last Parent Detached")]
    
    [Image(typeof(IconSocket), ColorTheme.Type.Blue, typeof(OverlayMinus))]
    [Description("A reference to the last Item that one of its Sockets was detached")]
    
    [Keywords("Socket", "Embed", "Enchant", "Remove")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemLastParentDetached : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => RuntimeItem.Socket_LastParentDetached;
        public override RuntimeItem Get(GameObject gameObject) => RuntimeItem.Socket_LastParentDetached;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastParentDetached instance = new GetRuntimeItemLastParentDetached();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Parent Detached]";
    }
}