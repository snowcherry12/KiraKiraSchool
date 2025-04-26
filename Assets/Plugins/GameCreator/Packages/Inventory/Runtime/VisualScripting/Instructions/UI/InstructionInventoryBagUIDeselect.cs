using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Version(0, 0, 1)]
    
    [Title("Deselect Item UI")]
    [Description("Deselects any currently selected item in the Bag UI")]
    
    [Category("Inventory/UI/Deselect Item UI")]
    
    [Keywords("Blur", "Select")]
    
    [Image(typeof(IconItem), ColorTheme.Type.TextLight, typeof(OverlayCross))]
    
    [Serializable]
    public class InstructionInventoryBagUIDeselect : Instruction
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "Deselect current UI Item";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            BagCellUI.Deselect();
            return DefaultResult;
        }
    }
}