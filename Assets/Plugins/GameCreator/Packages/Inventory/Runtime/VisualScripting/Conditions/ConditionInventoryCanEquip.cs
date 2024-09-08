using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Can Equip")]
    [Description("Returns true if the chosen Item can be equipped by the targeted Bag's wearer")]

    [Category("Inventory/Equipment/Can Equip")]
    
    [Parameter("Item", "The item type to check")]
    [Parameter("Bag", "The targeted Bag")]

    [Keywords("Inventory", "Contains", "Includes", "Wears", "Amount")]
    
    [Image(typeof(IconEquipment), ColorTheme.Type.Green)]
    [Serializable]
    public class ConditionInventoryCanEquip : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] protected PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"can Equip {this.m_Item} on {this.m_Bag}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Item item = this.m_Item.Get(args);

            Bag bag = this.m_Bag.Get<Bag>(args);
            return bag != null && bag.Equipment.CanEquipType(item);
        }
    }
}
