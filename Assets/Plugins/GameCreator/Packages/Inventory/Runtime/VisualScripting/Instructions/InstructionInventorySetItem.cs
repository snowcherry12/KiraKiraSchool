using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Set Item")]
    [Description("Saves an Item type on a Variable")]

    [Category("Inventory/Variables/Set Item")]
    
    [Parameter("Set", "The Variable that saves the Item")]
    [Parameter("Item", "The type of item saved")]

    [Keywords("Save", "Keep")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green, typeof(OverlayListVariable))]
    
    [Serializable]
    public class InstructionInventorySetItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertySetItem m_Set = new PropertySetItem();
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Set {this.m_Set} = {this.m_Item}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Item item = this.m_Item.Get(args);
            this.m_Set.Set(item, args);
            
            return DefaultResult;
        }
    }
}