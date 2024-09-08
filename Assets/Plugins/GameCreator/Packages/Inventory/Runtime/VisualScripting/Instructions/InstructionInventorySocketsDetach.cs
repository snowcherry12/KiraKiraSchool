using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Detach Runtime Item")]
    [Description("Detaches a Runtime Item from another Runtime Item socket")]

    [Category("Inventory/Sockets/Detach Runtime Item")]
    
    [Parameter("Runtime Item", "The item instance with an occupied socket")]
    [Parameter("Detach", "The item instance to detach from the other runtime item")]

    [Keywords("Bag", "Inventory", "Sockets")]
    [Keywords("Detach", "Disenchant")]
    
    [Image(typeof(IconSocket), ColorTheme.Type.Blue, typeof(OverlayMinus))]
    
    [Serializable]
    public class InstructionInventorySocketsDetach : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();
        [SerializeField] private PropertyGetRuntimeItem m_Detach = new PropertyGetRuntimeItem();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Detach {this.m_Detach} from {this.m_RuntimeItem}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            RuntimeItem detach = this.m_Detach.Get(args);

            if (runtimeItem == null || runtimeItem.Item == null) return DefaultResult;
            if (detach == null || detach.Item == null) return DefaultResult;
            
            Bag bag = runtimeItem.Bag;
            if (bag == null) return DefaultResult;
            
            RuntimeItem detachment = bag.Equipment.DetachFrom(runtimeItem, detach);
            if (detach.RuntimeID.Hash == detachment?.RuntimeID.Hash)
            {
                bag.Content.Add(detachment, true);
            }
            
            return DefaultResult;
        }
    }
}