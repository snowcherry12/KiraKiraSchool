using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Version(0, 0, 1)]

    [Title("Set Drop Amount")]
    [Description("Changes whether a Bag drops a single item or the whole stack when dropping them")]

    [Category("Inventory/UI/Set Drop Amount")]

    [Parameter("Drop", "Whether to drop one, or the whole stack")]

    [Keywords("Item", "Inventory", "Let", "Leave", "Take", "Place")]

    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow, typeof(OverlayArrowDown))]

    [Serializable]
    public class InstructionInventoryBagUIDropAmount : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private TBagUI.EnumDropAmount m_Drop = TBagUI.EnumDropAmount.One;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override string Title => $"Drop Amount to {this.m_Drop}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            TBagUI.DropAmount = this.m_Drop;
            return DefaultResult;
        }
    }
}
