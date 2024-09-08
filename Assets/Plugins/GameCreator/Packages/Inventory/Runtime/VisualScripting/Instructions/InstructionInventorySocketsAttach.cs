using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Attach Runtime Item")]
    [Description("Attaches a Runtime Item onto the first available Runtime Item socket")]

    [Category("Inventory/Sockets/Attach Runtime Item")]
    
    [Parameter("Runtime Item", "The item instance")]
    [Parameter("Attach", "The item instance attached to the other runtime item")]

    [Keywords("Bag", "Inventory", "Sockets")]
    [Keywords("Attach", "Enchant", "Embed", "Imbue")]
    
    [Image(typeof(IconSocket), ColorTheme.Type.Blue, typeof(OverlayPlus))]
    
    [Serializable]
    public class InstructionInventorySocketsAttach : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();
        [SerializeField] private PropertyGetRuntimeItem m_Attach = new PropertyGetRuntimeItem();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Attach {this.m_Attach} to {this.m_RuntimeItem}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            RuntimeItem attach = this.m_Attach.Get(args);

            if (runtimeItem == null || runtimeItem.Item == null) return DefaultResult;
            if (attach == null || attach.Item == null) return DefaultResult;

            Bag bag = runtimeItem.Bag;
            if (bag == null || bag != attach.Bag) return DefaultResult;
            
            bag.Equipment.AttachTo(runtimeItem, attach);
            return DefaultResult;
        }
    }
}