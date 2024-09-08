using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Title("On Open Bag UI")]
    [Category("Inventory/UI/On Open Bag UI")]
    [Description("Detects when a Bag UI is opened")]
    
    [Image(typeof(IconBagSolid), ColorTheme.Type.Green)]

    [Serializable]
    public class EventInventoryOnOpenBagUI : Event
    {
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            TBagUI.EventOpen -= this.OnOpenBagUI;
            TBagUI.EventOpen += this.OnOpenBagUI;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            TBagUI.EventOpen -= this.OnOpenBagUI;
        }

        private void OnOpenBagUI()
        {
            _ = this.m_Trigger.Execute(this.Self);
        }
    }
}