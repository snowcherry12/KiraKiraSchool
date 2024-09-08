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
    public class GetRuntimeItemLastAttachmentDetached : PropertyTypeGetRuntimeItem
    {
        public override RuntimeItem Get(Args args) => RuntimeItem.Socket_LastAttachmentDetached;
        public override RuntimeItem Get(GameObject gameObject) => RuntimeItem.Socket_LastAttachmentDetached;

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemLastAttachmentDetached instance = new GetRuntimeItemLastAttachmentDetached();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => "Item[Attachment Detached]";
    }
}