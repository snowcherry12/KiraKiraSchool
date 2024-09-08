using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory
{
    [Title("On Buy")]
    [Category("Inventory/Merchant/On Buy")]
    [Description("Executes after successfully purchasing an item from any Merchant")]
    
    [Image(typeof(IconMerchant), ColorTheme.Type.Purple, typeof(OverlayArrowRight))]

    [Serializable]
    public class EventInventoryOnBuy : Event
    {
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);

            Merchant.EventSellAny -= this.OnBuy;
            Merchant.EventSellAny += this.OnBuy;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            Merchant.EventSellAny -= this.OnBuy;
        }

        private void OnBuy()
        {
            _ = this.m_Trigger.Execute(this.Self);
        }
    }
}