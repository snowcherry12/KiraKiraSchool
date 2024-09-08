using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("On Add")]
    [Category("Inventory/On Add")]
    [Description("Executes after adding an item to the specified Bag")]
    
    [Keywords("Bag", "Inventory", "Item", "Add")]
    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow, typeof(OverlayPlus))]

    [Serializable]
    public class EventInventoryOnAddToBag : VisualScripting.Event
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
            
            this.Bag.Content.EventAdd -= this.OnAdd;
            this.Bag.Content.EventAdd += this.OnAdd;
        }

        protected override void OnStart(Trigger trigger)
        {
            base.OnStart(trigger);

            if (this.Bag != null)
            {
                this.Bag.Content.EventAdd -= this.OnAdd;
            }
            
            this.Bag = this.m_Bag.Get<Bag>(trigger);
            if (this.Bag == null) return;

            this.Args = new Args(this.Self, this.Bag.gameObject);
            
            this.Bag.Content.EventAdd -= this.OnAdd;
            this.Bag.Content.EventAdd += this.OnAdd;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (this.Bag == null) return;
            this.Bag.Content.EventAdd -= this.OnAdd;
        }

        private void OnAdd(RuntimeItem runtimeItem)
        {
            if (!this.m_Filter.Match(runtimeItem?.Item, this.Args)) return;
            _ = this.m_Trigger.Execute(this.Args);
        }
    }
}