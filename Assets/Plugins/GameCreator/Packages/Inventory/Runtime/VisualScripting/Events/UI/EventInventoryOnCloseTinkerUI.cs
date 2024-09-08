using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Title("On Close Tinker UI")]
    [Category("Inventory/UI/On Close Tinker UI")]
    [Description("Detects when a Tinker UI is closed")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Red)]

    [Serializable]
    public class EventInventoryOnCloseTinkerUI : Event
    {
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            TinkerUI.EventClose -= this.OnCloseTinkerUI;
            TinkerUI.EventClose += this.OnCloseTinkerUI;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            TinkerUI.EventClose -= this.OnCloseTinkerUI;
        }

        private void OnCloseTinkerUI()
        {
            _ = this.m_Trigger.Execute(this.Self);
        }
    }
}