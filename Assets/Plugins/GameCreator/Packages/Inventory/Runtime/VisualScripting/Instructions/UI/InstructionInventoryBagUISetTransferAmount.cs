using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Version(0, 0, 1)]
    
    [Title("Set Transfer Amount")]
    [Description("Changes whether a Bag moves a single item or the whole stack when transferring them")]

    [Category("Inventory/UI/Set Transfer Amount")]
    
    [Parameter("Transfer", "Whether to transfer one, or the whole stack")]

    [Keywords("Item", "Inventory", "Transfer", "Move", "Content", "Place")]

    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    
    [Serializable]
    public class InstructionInventoryBagUISetTransferAmount : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private TBagUI.EnumTransferAmount m_Transfer = TBagUI.EnumTransferAmount.One;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Transfer items move {this.m_Transfer}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            TBagUI.TransferAmount = this.m_Transfer;
            return DefaultResult;
        }
    }
}