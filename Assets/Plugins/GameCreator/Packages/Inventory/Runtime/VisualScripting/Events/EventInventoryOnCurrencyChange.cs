using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Title("On Change Currency")]
    [Category("Inventory/Currency/On Change Currency")]
    [Description("Detects when a Bag's Currency value changes")]

    [Image(typeof(IconCurrency), ColorTheme.Type.Yellow)]

    [Serializable]
    public class EventInventoryOnCurrencyChange : VisualScripting.Event
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] private Bag Bag   { get; set; }
        [field: NonSerialized] private Args Args { get; set; }
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            
            this.Bag = this.m_Bag.Get<Bag>(trigger);
            if (this.Bag == null) return;

            this.Args = new Args(this.Self, this.Bag.gameObject);
            
            this.Bag.Wealth.EventChange -= this.OnCurrency;
            this.Bag.Wealth.EventChange += this.OnCurrency;
        }
        
        protected override void OnStart(Trigger trigger)
        {
            base.OnStart(trigger);

            if (this.Bag != null)
            {
                this.Bag.Wealth.EventChange -= this.OnCurrency;
            }
            
            this.Bag = this.m_Bag.Get<Bag>(trigger);
            if (this.Bag == null) return;

            this.Args = new Args(this.Self, this.Bag.gameObject);
            
            this.Bag.Wealth.EventChange -= this.OnCurrency;
            this.Bag.Wealth.EventChange += this.OnCurrency;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (this.Bag == null) return;
            this.Bag.Wealth.EventChange -= this.OnCurrency;
        }

        private void OnCurrency(IdString idString, int previousAmount, int newAmount)
        {
            _ = this.m_Trigger.Execute(this.Args);
        }
    }
}
