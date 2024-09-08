using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Can Add")]
    [Description("Returns true if the item can be added to the Bag component")]

    [Category("Inventory/Can Add")]
    
    [Parameter("Item", "The item type to add")]
    [Parameter("To Bag", "The target destination Bag")]

    [Keywords("Inventory", "Give", "Put", "Set")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Yellow, typeof(OverlayArrowLeft))]
    [Serializable]
    public class ConditionInventoryCanAdd : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetItem m_Item = new PropertyGetItem();
        [SerializeField] protected PropertyGetGameObject m_ToBag = GetGameObjectPlayer.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"can add {this.m_Item} to {this.m_ToBag}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Item item = this.m_Item.Get(args);
            Bag toBag = this.m_ToBag.Get<Bag>(args);
            
            return toBag != null && toBag.Content.CanAddType(item, true);
        }
    }
}
