using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using Event = GameCreator.Runtime.VisualScripting.Event;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Title("On Socket Attach")]
    [Category("Inventory/Sockets/On Socket Attach")]
    [Description("Detects when an Item's Socket gets another Item attached")]
    
    [Image(typeof(IconSocket), ColorTheme.Type.Yellow, typeof(OverlayPlus))]

    [Serializable]
    public class EventInventoryOnSocketAttach : Event
    {
        [SerializeField] private AnyOrItem m_OnItem = new AnyOrItem();
        [SerializeField] private AnyOrItem m_OnAttachment = new AnyOrItem();
        
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            RuntimeSockets.EventAttachRuntimeItem -= this.Callback;
            RuntimeSockets.EventAttachRuntimeItem += this.Callback;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            RuntimeSockets.EventAttachRuntimeItem -= this.Callback;
        }
        
        private void Callback(RuntimeItem runtimeItem, RuntimeItem attachment)
        {
            Args args = new Args(
                this.m_Trigger.gameObject,
                runtimeItem.Bag != null
                    ? runtimeItem.Bag.gameObject
                    : this.m_Trigger.gameObject
            );
            
            if (!this.m_OnItem.Match(runtimeItem.Item, args)) return;
            if (!this.m_OnAttachment.Match(attachment.Item, args)) return;
            
            _ = this.m_Trigger.Execute(this.Self);   
        }
    }
}