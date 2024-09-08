using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Can Dismantle")]
    [Description("Returns true if the item can be dismantled")]

    [Category("Inventory/Tinker/Can Dismantle")]
    
    [Parameter("From Bag", "The Bag where item is picked")]
    [Parameter("Item", "The item type attempted to dismantle")]
    [Parameter("To Bag", "The destination Bag for all ingredients after dismantling the Item")]

    [Keywords("Inventory", "Apart", "Disassemble", "Deconstruct", "Tear", "Separate")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Red)]
    [Serializable]
    public class ConditionInventoryCanDismantle : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_FromBag = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] private PropertyGetGameObject m_ToBag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"can dismantle {this.m_Item} from {this.m_FromBag}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Bag fromBag = this.m_FromBag.Get<Bag>(args);
            Item item = this.m_Item.Get(args);
            Bag toBag = this.m_ToBag.Get<Bag>(args);

            if (fromBag == null) return false;
            if (item == null) return false;
            if (toBag == null) return false;
            
            return Crafting.CanDismantle(item, fromBag, toBag);
        }
    }
}
