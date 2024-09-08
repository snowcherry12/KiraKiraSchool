using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("On Remove")]
    [Category("Inventory/On Remove")]
    [Description("Executes after removing an item from the specified Bag")]
    
    [Keywords("Bag", "Inventory", "Item", "Take")]
    [Image(typeof(IconBagSolid), ColorTheme.Type.Red, typeof(OverlayMinus))]

    [Serializable]
    public class EventInventoryOnRemoveFromBag : VisualScripting.Event
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        [SerializeField] private AnyOrItem m_Filter = new AnyOrItem();
        
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
            
            this.Bag.Content.EventRemove -= this.OnRemove;
            this.Bag.Content.EventRemove += this.OnRemove;
        }
        
        protected override void OnStart(Trigger trigger)
        {
            base.OnStart(trigger);

            if (this.Bag != null)
            {
                this.Bag.Content.EventRemove -= this.OnRemove;
            }
            
            this.Bag = this.m_Bag.Get<Bag>(trigger);
            if (this.Bag == null) return;

            this.Args = new Args(this.Self, this.Bag.gameObject);
            
            this.Bag.Content.EventRemove -= this.OnRemove;
            this.Bag.Content.EventRemove += this.OnRemove;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (this.Bag == null) return;
            this.Bag.Content.EventRemove -= this.OnRemove;
        }

        private void OnRemove(RuntimeItem runtimeItem)
        {
            if (!this.m_Filter.Match(runtimeItem?.Item, this.Args)) return;
            _ = this.m_Trigger.Execute(this.Args);
        }
    }
}