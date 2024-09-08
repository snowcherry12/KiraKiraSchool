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
    public class GetItemLastParentAttached : PropertyTypeGetItem
    {
        public override Item Get(Args args) => RuntimeItem.Socket_LastParentAttached?.Item;
        public override Item Get(GameObject gameObject) => RuntimeItem.Socket_LastParentAttached?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastParentAttached instance = new GetItemLastParentAttached();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Parent Attached]";
    }
}