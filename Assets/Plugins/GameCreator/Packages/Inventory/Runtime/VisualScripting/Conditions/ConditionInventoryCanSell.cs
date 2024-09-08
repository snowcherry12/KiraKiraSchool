using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Can Sell")]
    [Description("Returns true if the item can be sold to a Merchant")]

    [Category("Inventory/Merchant/Can Sell")]
    
    [Parameter("From Bag", "The Bag where the item is sold")]
    [Parameter("Item", "The item type attempted to sell")]
    [Parameter("To Merchant", "The Merchant target")]

    [Keywords("Inventory", "Vend", "Trade", "Exchange", "Part", "Bargain", "Haggle")]
    
    [Image(typeof(IconMerchant), ColorTheme.Type.Purple, typeof(OverlayArrowLeft))]
    [Serializable]
    public class ConditionInventoryCanSell : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_FromBag = GetGameObjectPlayer.Create();
        
        [SerializeField]
        private PropertyGetItem m_Item = new PropertyGetItem();

        [SerializeField]
        private PropertyGetGameObject m_ToMerchant = GetGameObjectInventoryMerchant.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"can sell {this.m_Item} to {this.m_ToMerchant}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Bag fromBag = this.m_FromBag.Get<Bag>(args);
            Item item = this.m_Item.Get(args);
            Merchant toMerchant = this.m_ToMerchant.Get<Merchant>(args);
            
            if (fromBag == null) return false;
            if (item == null) return false;
            if (toMerchant == null || toMerchant.Bag == null) return false;
            
            Cell[] cells = fromBag.Content.GetTypes(item);
            foreach (Cell cell in cells)
            {
                RuntimeItem runtimeItem = cell.Peek();
                if (toMerchant.CanBuyFromClient(fromBag, runtimeItem)) return true;
            }

            return false;
        }
    }
}
