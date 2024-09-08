using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Set Runtime Item")]
    [Description("Saves a Runtime Item on a Variable")]

    [Category("Inventory/Variables/Set Runtime Item")]
    
    [Parameter("Set", "The Variable that saves the Runtime Item")]
    [Parameter("Runtime Item", "The Item instance saved")]

    [Keywords("Save", "Keep")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Blue, typeof(OverlayListVariable))]
    
    [Serializable]
    public class InstructionInventorySetRuntimeItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertySetRuntimeItem m_Set = new PropertySetRuntimeItem();
        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Set {this.m_Set} = {this.m_RuntimeItem}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            this.m_Set.Set(runtimeItem, args);
            
            return DefaultResult;
        }
    }
}