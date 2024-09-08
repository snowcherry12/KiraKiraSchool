using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory
{
    [Title("On Instantiate Item")]
    [Category("Inventory/On Instantiate Item")]
    [Description("Executes after dropping an item from a Bag to the scene")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue, typeof(OverlayArrowDown))]

    [Serializable]
    public class EventInventoryOnInstantiateItem : Event
    {
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);

            Item.EventInstantiate -= this.OnInstantiate;
            Item.EventInstantiate += this.OnInstantiate;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            Item.EventInstantiate -= this.OnInstantiate;
        }

        private void OnInstantiate()
        {
            _ = this.m_Trigger.Execute(this.Self);
        }
    }
}