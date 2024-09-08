using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory
{
    [Title("On Sell")]
    [Category("Inventory/Merchant/On Sell")]
    [Description("Executes after successfully selling an item to any Merchant")]
    
    [Image(typeof(IconMerchant), ColorTheme.Type.Purple, typeof(OverlayArrowLeft))]

    [Serializable]
    public class EventInventoryOnSell : Event
    {
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);

            Merchant.EventBuyAny -= this.OnSell;
            Merchant.EventBuyAny += this.OnSell;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            Merchant.EventBuyAny -= this.OnSell;
        }

        private void OnSell()
        {
            _ = this.m_Trigger.Execute(this.Self);
        }
    }
}