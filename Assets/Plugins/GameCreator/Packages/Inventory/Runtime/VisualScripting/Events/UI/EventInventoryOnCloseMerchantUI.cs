using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Title("On Close Merchant UI")]
    [Category("Inventory/UI/On Close Merchant UI")]
    [Description("Detects when a Merchant UI is closed")]
    
    [Image(typeof(IconMerchant), ColorTheme.Type.Red)]

    [Serializable]
    public class EventInventoryOnCloseMerchantUI : Event
    {
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            MerchantUI.EventClose -= this.OnCloseMerchantUI;
            MerchantUI.EventClose += this.OnCloseMerchantUI;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            MerchantUI.EventClose -= this.OnCloseMerchantUI;
        }

        private void OnCloseMerchantUI()
        {
            _ = this.m_Trigger.Execute(this.Self);
        }
    }
}