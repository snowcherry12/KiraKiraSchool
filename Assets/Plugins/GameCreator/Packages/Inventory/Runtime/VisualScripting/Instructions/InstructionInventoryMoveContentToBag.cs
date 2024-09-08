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
    
    [Title("Move Content to Bag")]
    [Description("Moves all the contents of a Bag to another Bag")]

    [Category("Inventory/Bags/Move Content to Bag")]
    
    [Parameter("From Bag", "The Bag component where its contents are removed")]
    [Parameter("To Bag", "The targeted Bag component where the contents end up")]

    [Keywords("Bag", "Inventory", "Container", "Stash", "Chest", "Take", "All")]
    [Keywords("Give", "Take", "Borrow", "Lend", "Buy", "Purchase", "Sell", "Steal", "Rob")]
    
    [Image(typeof(IconBagOutline), ColorTheme.Type.Green, typeof(OverlayArrowRight))]
    
    [Serializable]
    public class InstructionInventoryMoveContentToBag : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_FromBag = GetGameObjectInventoryBag.Create();
        [SerializeField] private PropertyGetGameObject m_ToBag = GetGameObjectPlayer.Create();
        [SerializeField] private bool m_MoveWealth;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Move from {this.m_FromBag} to {this.m_ToBag}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Bag fromBag = this.m_FromBag.Get<Bag>(args);
            if (fromBag == null) return DefaultResult;
            
            Bag toBag = this.m_ToBag.Get<Bag>(args);
            if (toBag == null) return DefaultResult;

            List<Cell> content = fromBag.Content.CellList;
            foreach (Cell fromCell in content)
            {
                if (fromCell == null || fromCell.Available) continue;

                List<IdString> fromCellList = fromCell.List;
                for (int i = fromCellList.Count - 1; i >= 0; --i)
                {
                    IdString runtimeItemID = fromCellList[i];
                    RuntimeItem runtimeItem = fromBag.Content.GetRuntimeItem(runtimeItemID);
                    
                    if (toBag.Content.CanAdd(runtimeItem, true))
                    {
                        fromBag.Content.Remove(runtimeItem);
                        if (toBag.Content.Add(runtimeItem, true) == TBagContent.INVALID)
                        {
                            fromBag.Content.Add(runtimeItem, true);
                        }
                    }
                }
            }

            if (this.m_MoveWealth)
            {
                List<IdString> wealth = fromBag.Wealth.List;
                foreach (IdString currencyID in wealth)
                {
                    int amount = fromBag.Wealth.Get(currencyID);
                    
                    fromBag.Wealth.Subtract(currencyID, amount);
                    toBag.Wealth.Add(currencyID, amount);
                }
            }
            
            return DefaultResult;
        }
    }
}