using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Title("On Drop Item")]
    [Category("Inventory/On Drop Item")]
    [Description("Detects when a Bag's item is dropped onto the Trigger")]

    [Image(typeof(IconItem), ColorTheme.Type.Yellow, typeof(OverlayArrowDown))]

    [Serializable]
    public class EventInventoryOnDropItem : VisualScripting.Event
    {
        public static readonly PropertyName COMMAND_DROP_ITEM = "on-drop-item";

        public override bool RequiresCollider => true;

        protected override void OnReceiveCommand(Trigger trigger, CommandArgs args)
        {
            base.OnReceiveCommand(trigger, args);
            
            if (args.Command != COMMAND_DROP_ITEM) return;
            _ = trigger.Execute(args.Target);
        }
    }
}
