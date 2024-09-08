using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Version(0, 0, 1)]

    [Title("Set Split Amount")]
    [Description("Changes whether a Bag splits by unstacking a single item or the whole stack is split in half")]

    [Category("Inventory/UI/Set Split Amount")]

    [Parameter("Drop", "Whether to split one, or the whole stack in half")]

    [Keywords("Item", "Inventory", "Stack", "Unstack", "Split", "Divide")]

    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow, typeof(OverlayDot))]

    [Serializable]
    public class InstructionInventoryBagUISplitAmount : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private TBagUI.EnumSplitAmount m_Split = TBagUI.EnumSplitAmount.One;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Split items in {this.m_Split}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            TBagUI.SplitAmount = this.m_Split;
            return DefaultResult;
        }
    }
}
