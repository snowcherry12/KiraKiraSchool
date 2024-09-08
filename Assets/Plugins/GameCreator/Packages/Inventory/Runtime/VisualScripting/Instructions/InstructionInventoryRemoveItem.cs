using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(0, 0, 1)]
    
    [Title("Remove Item")]
    [Description("Removes an Item from the specified Bag")]

    [Category("Inventory/Bags/Remove Item")]
    
    [Parameter("Item", "The parent type of item to be removed")]
    [Parameter("Bag", "The targeted Bag component")]

    [Keywords("Bag", "Inventory", "Container", "Stash")]
    [Keywords("Give", "Take", "Borrow", "Lend", "Buy", "Purchase", "Sell", "Steal", "Rob")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green, typeof(OverlayMinus))]
    
    [Serializable]
    public class InstructionInventoryRemoveItem : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Remove {this.m_Item} from {this.m_Bag}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Item item = this.m_Item.Get(args);
            if (item == null) return DefaultResult;
            
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return DefaultResult;

            bag.Content.RemoveType(item);
            return DefaultResult;
        }
    }
}