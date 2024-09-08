using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Can Buy")]
    [Description("Returns true if the item can be bought from a Merchant")]

    [Category("Inventory/Merchant/Can Buy")]
    
    [Parameter("From Merchant", "The Merchant component")]
    [Parameter("Item", "The item type attempted to purchase")]
    [Parameter("To Bag", "The destination Bag for the item")]

    [Keywords("Inventory", "Purchase", "Get", "Bargain", "Haggle")]
    
    [Image(typeof(IconMerchant), ColorTheme.Type.Purple, typeof(OverlayArrowRight))]
    [Serializable]
    public class ConditionInventoryCanBuy : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_FromMerchant = GetGameObjectInventoryMerchant.Create();
        
        [SerializeField]
        private PropertyGetItem m_Item = new PropertyGetItem();
        
        [SerializeField]
        private PropertyGetGameObject m_ToBag = GetGameObjectPlayer.Create();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"can buy {this.m_Item} from {this.m_FromMerchant}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Merchant fromMerchant = this.m_FromMerchant.Get<Merchant>(args);
            Item item = this.m_Item.Get(args);
            Bag toBag = this.m_ToBag.Get<Bag>(args);

            if (fromMerchant == null || fromMerchant.Bag == null) return false;
            if (item == null) return false;
            if (toBag == null) return false;

            Cell[] cells = fromMerchant.Bag.Content.GetTypes(item);
            foreach (Cell cell in cells)
            {
                RuntimeItem runtimeItem = cell.Peek();
                if (fromMerchant.CanSellToClient(toBag, runtimeItem)) return true;
            }

            return false;
        }
    }
}
