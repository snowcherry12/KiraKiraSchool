using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Add Runtime Item")]
    [Description("Adds an existing instance of an Item and adds it to the specified Bag")]

    [Category("Inventory/Bags/Add Runtime Item")]
    
    [Parameter("Runtime Item", "The existing Item instance")]
    [Parameter("Bag", "The targeted Bag component")]

    [Keywords("Bag", "Inventory", "Container", "Stash")]
    [Keywords("Give", "Take", "Borrow", "Lend", "Buy", "Purchase", "Sell", "Steal", "Rob")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Blue, typeof(OverlayPlus))]
    
    [Serializable]
    public class InstructionInventoryAddRuntimeItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Add {this.m_RuntimeItem} to {this.m_Bag}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null) return DefaultResult;
            
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return DefaultResult;

            bag.Content.Add(runtimeItem, true);
            return DefaultResult;
        }
    }
}