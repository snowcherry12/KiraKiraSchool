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
    public class GetRuntimeItemLastAttachmentAttached : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => RuntimeItem.Socket_LastAttachmentAttached;
        public override RuntimeItem Get(GameObject gameObject) => RuntimeItem.Socket_LastAttachmentAttached;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastAttachmentAttached instance = new GetRuntimeItemLastAttachmentAttached();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Attachment Attached]";
    }
}