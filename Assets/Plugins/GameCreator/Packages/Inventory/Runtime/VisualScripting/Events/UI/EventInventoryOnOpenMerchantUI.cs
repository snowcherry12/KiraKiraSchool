using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Title("On Open Merchant UI")]
    [Category("Inventory/UI/On Open Merchant UI")]
    [Description("Detects when a Merchant UI is opened")]
    
    [Image(typeof(IconMerchant), ColorTheme.Type.Green)]

    [Serializable]
    public class EventInventoryOnOpenMerchantUI : Event
    {
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            MerchantUI.EventOpen -= this.OnOpenMerchantUI;
            MerchantUI.EventOpen += this.OnOpenMerchantUI;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            MerchantUI.EventOpen -= this.OnOpenMerchantUI;
        }

        private void OnOpenMerchantUI()
        {
            _ = this.m_Trigger.Execute(this.Self);
        }
    }
}