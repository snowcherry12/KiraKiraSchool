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
    public class GetItemLastParentDetached : PropertyTypeGetItem
    {
        public override Item Get(Args args) => RuntimeItem.Socket_LastParentDetached?.Item;
        public override Item Get(GameObject gameObject) => RuntimeItem.Socket_LastParentDetached?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastParentDetached instance = new GetItemLastParentDetached();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Parent Detached]";
    }
}