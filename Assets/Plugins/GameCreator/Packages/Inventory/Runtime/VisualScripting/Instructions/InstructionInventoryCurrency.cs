using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Change Currency")]
    [Description("Modifies the value of a Bag's currency")]

    [Category("Inventory/Currency/Change Currency")]
    
    [Parameter("Currency", "The currency type to modify")]
    [Parameter("Amount", "The value and operation performed")]
    [Parameter("Bag", "The targeted Bag component")]
    
    [Keywords("Bag", "Inventory", "Container", "Stash")]
    [Keywords("Give", "Take", "Borrow", "Lend", "Buy", "Purchase", "Sell", "Steal", "Rob")]
    [Keywords("Coin", "Cash", "Bill", "Value", "Money")]
    
    [Image(typeof(IconCurrency), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class InstructionInventoryCurrency : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private Currency m_Currency;
        [SerializeField] private ChangeInteger m_Amount = new ChangeInteger(100);
        
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "{0} {1} {2}",
            this.m_Currency != null ? this.m_Currency.name : "(none)",
            this.m_Bag,
            this.m_Amount
        );

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            if (this.m_Currency == null) return DefaultResult;
            
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return DefaultResult;

            int prevAmount = bag.Wealth.Get(this.m_Currency);
            int newAmount = this.m_Amount.Get(prevAmount, args);
            
            bag.Wealth.Set(this.m_Currency, newAmount);
            return DefaultResult;
        }
    }
}