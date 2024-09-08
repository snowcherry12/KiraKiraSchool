using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Attachment Detached")]
    [Category("Sockets/Last Attachment Detached")]
    
    [Image(typeof(IconSocket), ColorTheme.Type.Yellow, typeof(OverlayMinus))]
    [Description("A reference to the last Item detached from a Socket")]
    
    [Keywords("Socket", "Embed", "Enchant", "Remove")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastAttachmentDetached : PropertyTypeGetItem
    {
        public override Item Get(Args args) => RuntimeItem.Socket_LastAttachmentDetached?.Item;
        public override Item Get(GameObject gameObject) => RuntimeItem.Socket_LastAttachmentDetached?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastAttachmentDetached instance = new GetItemLastAttachmentDetached();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Attachment Detached]";
    }
}