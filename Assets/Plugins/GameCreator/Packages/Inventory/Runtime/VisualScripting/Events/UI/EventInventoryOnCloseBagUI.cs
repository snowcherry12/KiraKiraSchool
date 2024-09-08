using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Title("On Close Bag UI")]
    [Category("Inventory/UI/On Close Bag UI")]
    [Description("Detects when a Bag UI is closed")]
    
    [Image(typeof(IconBagSolid), ColorTheme.Type.Red)]

    [Serializable]
    public class EventInventoryOnCloseBagUI : Event
    {
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            TBagUI.EventClose -= this.OnCloseBagUI;
            TBagUI.EventClose += this.OnCloseBagUI;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            TBagUI.EventClose -= this.OnCloseBagUI;
        }

        private void OnCloseBagUI()
        {
            _ = this.m_Trigger.Execute(this.Self);
        }
    }
}