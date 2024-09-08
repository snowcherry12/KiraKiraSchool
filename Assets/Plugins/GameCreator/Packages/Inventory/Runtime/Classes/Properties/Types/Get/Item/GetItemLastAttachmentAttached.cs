using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Attachment Attached")]
    [Category("Sockets/Last Attachment Attached")]
    
    [Image(typeof(IconSocket), ColorTheme.Type.Yellow, typeof(OverlayPlus))]
    [Description("A reference to the last Item attached to a Socket")]
    
    [Keywords("Socket", "Embed", "Enchant", "Add")]

    [Serializable] [HideLabelsInEditor]
    public class GetItemLastAttachmentAttached : PropertyTypeGetItem
    {
        public override Item Get(Args args) => RuntimeItem.Socket_LastAttachmentAttached?.Item;
        public override Item Get(GameObject gameObject) => RuntimeItem.Socket_LastAttachmentAttached?.Item;

        public static PropertyGetItem Create()
        {
            GetItemLastAttachmentAttached instance = new GetItemLastAttachmentAttached();
            return new PropertyGetItem(instance);
        }

        public override string String => "Item[Attachment Attached]";
    }
}