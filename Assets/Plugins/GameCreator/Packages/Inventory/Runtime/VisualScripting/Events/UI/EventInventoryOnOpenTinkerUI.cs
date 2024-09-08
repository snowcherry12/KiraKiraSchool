using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Title("On Open Tinker UI")]
    [Category("Inventory/UI/On Open Tinker UI")]
    [Description("Detects when a Tinker UI is opened")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Green)]

    [Serializable]
    public class EventInventoryOnOpenTinkerUI : Event
    {
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            TinkerUI.EventOpen -= this.OnOpenTinkerUI;
            TinkerUI.EventOpen += this.OnOpenTinkerUI;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            TinkerUI.EventOpen -= this.OnOpenTinkerUI;
        }

        private void OnOpenTinkerUI()
        {
            _ = this.m_Trigger.Execute(this.Self);
        }
    }
}