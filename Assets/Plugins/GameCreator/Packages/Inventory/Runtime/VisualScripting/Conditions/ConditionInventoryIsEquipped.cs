using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Is Equipped")]
    [Description("Returns true if the Bag's wearer has an Item of that type currently equipped")]

    [Category("Inventory/Equipment/Is Equipped")]
    
    [Parameter("Item", "The item type to check")]
    [Parameter("Bag", "The targeted Bag")]

    [Keywords("Inventory", "Wears")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Blue)]
    [Serializable]
    public class ConditionInventoryIsEquipped : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] protected PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_Item} Equipped on {this.m_Bag}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Item item = this.m_Item.Get(args);

            Bag bag = this.m_Bag.Get<Bag>(args);
            return bag != null && bag.Equipment.IsEquippedType(item);
        }
    }
}
