using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory
{
    [Title("On Craft")]
    [Category("Inventory/Tinker/On Craft")]
    [Description("Executes right after successfully crafting any item")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Teal)]

    [Serializable]
    public class EventInventoryOnCraft : Event
    {
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);

            Crafting.EventCraft -= this.OnCraft;
            Crafting.EventCraft += this.OnCraft;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            Crafting.EventCraft -= this.OnCraft;
        }

        private void OnCraft()
        {
            _ = this.m_Trigger.Execute(this.Self);
        }
    }
}