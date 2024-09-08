using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory
{
    [Title("On Dismantle")]
    [Category("Inventory/Tinker/On Dismantle")]
    [Description("Executes right after successfully dismantling any item")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Red)]

    [Serializable]
    public class EventInventoryOnDismantle : Event
    {
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);

            Crafting.EventDismantle -= this.OnDismantle;
            Crafting.EventDismantle += this.OnDismantle;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            Crafting.EventDismantle -= this.OnDismantle;
        }

        private void OnDismantle()
        {
            _ = this.m_Trigger.Execute(this.Self);
        }
    }
}