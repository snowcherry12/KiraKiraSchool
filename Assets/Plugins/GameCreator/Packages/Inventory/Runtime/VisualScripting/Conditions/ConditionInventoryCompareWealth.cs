using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Compare Wealth")]
    [Description("Returns true if a comparison between the wealth and another integer is satisfied")]

    [Category("Inventory/Wealth/Compare Wealth")]
    
    [Parameter("Bag", "The Bag component with the Wealth being compared")]
    [Parameter("Currency", "The currency type to compare")]
    [Parameter("Comparison", "The comparison operation performed between both values")]
    [Parameter("Compare To", "The integer value that is compared against")]
    
    [Keywords("Price", "Money", "Cash", "Currency", "Coin", "Gold")]
    [Image(typeof(IconCurrency), ColorTheme.Type.Yellow)]
    [Serializable]
    public class ConditionInventoryCompareWealth : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectInventoryBag.Create();
        [SerializeField] private Currency m_Currency = null;

        [SerializeField] private CompareInteger m_CompareTo = new CompareInteger();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Bag} {this.m_CompareTo}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return false;
            
            int value = bag.Wealth.Get(this.m_Currency);
            return this.m_CompareTo.Match(value, args);
        }
    }
}
