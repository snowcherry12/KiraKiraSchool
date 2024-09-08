using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Move Wealth to Bag")]
    [Description("Moves all wealth from one Bag to another one")]

    [Category("Inventory/Bags/Move Wealth to Bag")]
    
    [Parameter("From Bag", "The Bag component where its wealth is taken from")]
    [Parameter("To Bag", "The targeted Bag component where the wealth ends up")]

    [Keywords("Bag", "Inventory", "Container", "Stash", "Chest", "Take", "All")]
    [Keywords("Give", "Take", "Borrow", "Lend", "Buy", "Purchase", "Sell", "Steal", "Rob")]
    [Keywords("Currency", "Cash", "Money", "Coins")]
    
    [Image(typeof(IconCurrency), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    
    [Serializable]
    public class InstructionInventoryMoveWealthToBag : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_FromBag = GetGameObjectInventoryBag.Create();
        [SerializeField] private PropertyGetGameObject m_ToBag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Move Wealth from {this.m_FromBag} to {this.m_ToBag}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Bag fromBag = this.m_FromBag.Get<Bag>(args);
            if (fromBag == null) return DefaultResult;
            
            Bag toBag = this.m_ToBag.Get<Bag>(args);
            if (toBag == null) return DefaultResult;

            List<IdString> wealth = fromBag.Wealth.List;
            foreach (IdString currencyID in wealth)
            {
                int amount = fromBag.Wealth.Get(currencyID);
                    
                fromBag.Wealth.Subtract(currencyID, amount);
                toBag.Wealth.Add(currencyID, amount);
            }
            
            return DefaultResult;
        }
    }
}