using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Has Item")]
    [Description("Returns true if the Bag component contains, at least, the specified amount of an item")]

    [Category("Inventory/Has Item")]
    
    [Parameter("Item", "The item type to check")]
    [Parameter("Amount", "The minimum amount of a particular item")]
    
    [Parameter("Bag", "The targeted Bag")]

    [Keywords("Inventory", "Contains", "Includes", "Wears", "Amount")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green)]
    [Serializable]
    public class ConditionInventoryHasItem : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private PropertyGetInteger m_Amount = GetDecimalInteger.Create(1);
        
        [SerializeField] protected PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"has {this.m_Amount} {this.m_Item} in {this.m_Bag}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Item item = this.m_Item.Get(args);
            int amount = (int) this.m_Amount.Get(args);
            
            Bag bag = this.m_Bag.Get<Bag>(args);
            return bag != null && bag.Content.ContainsType(item, amount);
        }
    }
}
