using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("On Equip")]
    [Category("Inventory/Equipment/On Equip")]
    [Description("Executes after equipping an item from the specified Bag")]
    
    [Keywords("Bag", "Inventory", "Item", "Add", "Wear")]
    [Image(typeof(IconEquipment), ColorTheme.Type.Blue, typeof(OverlayPlus))]

    [Serializable]
    public class EventInventoryOnEquip : VisualScripting.Event
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
            
            this.Bag.Equipment.EventEquip -= this.OnEquip;
            this.Bag.Equipment.EventEquip += this.OnEquip;
        }
        
        protected override void OnStart(Trigger trigger)
        {
            base.OnStart(trigger);

            if (this.Bag != null)
            {
                this.Bag.Equipment.EventEquip -= this.OnEquip;
            }
            
            this.Bag = this.m_Bag.Get<Bag>(trigger);
            if (this.Bag == null) return;

            this.Args = new Args(this.Self, this.Bag.gameObject);
            
            this.Bag.Equipment.EventEquip -= this.OnEquip;
            this.Bag.Equipment.EventEquip += this.OnEquip;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (this.Bag == null) return;
            this.Bag.Equipment.EventEquip -= this.OnEquip;
        }

        private void OnEquip(RuntimeItem runtimeItem, int equipIndex)
        {
            _ = this.m_Trigger.Execute(this.Args);
        }
    }
}