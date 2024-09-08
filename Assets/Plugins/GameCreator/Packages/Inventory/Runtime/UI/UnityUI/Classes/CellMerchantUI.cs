using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Serializable]
    public class CellMerchantUI
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Button m_ButtonBuy;
        [SerializeField] private PriceUI m_BuyPrice;
        
        [SerializeField] private Button m_ButtonSell;
        [SerializeField] private PriceUI m_SellPrice;

        [SerializeField] private GameObject m_InactiveIfEmpty;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private MerchantUI m_MerchantUI;
        [NonSerialized] private RuntimeItem m_RuntimeItem;
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventTrade;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void RefreshUI(TBagUI bagUI, Vector2Int position)
        {
            if (bagUI.MerchantUI == null) return;
            
            Cell cell = bagUI.Bag.Content.GetContent(position);
            if (cell == null || cell.Available) return;
            
            this.m_MerchantUI = bagUI.MerchantUI;
            this.m_RuntimeItem = cell.Peek();

            if (this.m_ButtonBuy != null)
            {
                bool canBuy = bagUI.MerchantUI.Merchant.CanSellToClient(
                    bagUI.MerchantUI.ClientBag,
                    cell.Peek()
                );

                this.m_ButtonBuy.interactable = canBuy;
                this.m_ButtonBuy.onClick.RemoveListener(this.Buy);
                this.m_ButtonBuy.onClick.AddListener(this.Buy);
            }

            if (this.m_ButtonSell != null)
            {
                bool canSell = bagUI.MerchantUI.Merchant.CanBuyFromClient(
                    bagUI.MerchantUI.ClientBag,
                    cell.Peek()
                );

                this.m_ButtonSell.interactable = canSell;
                this.m_ButtonSell.onClick.RemoveListener(this.Sell);
                this.m_ButtonSell.onClick.AddListener(this.Sell);
            }

            if (this.m_BuyPrice != null)
            {
                int buyPrice = this.m_MerchantUI.Merchant.GetBuyPrice(
                    this.m_RuntimeItem,
                    this.m_MerchantUI.ClientBag
                );
                
                this.m_BuyPrice.RefreshUI(this.m_RuntimeItem.Item.Price.Currency, buyPrice);
            }
            
            if (this.m_SellPrice != null)
            {
                int sellPrice = this.m_MerchantUI.Merchant.GetSellPrice(
                    this.m_RuntimeItem,
                    this.m_MerchantUI.ClientBag
                );
                
                this.m_SellPrice.RefreshUI(this.m_RuntimeItem.Item.Price.Currency, sellPrice);
            }

            if (this.m_InactiveIfEmpty != null)
            {
                this.m_InactiveIfEmpty.SetActive(this.m_RuntimeItem != null);
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void Buy()
        {
            this.m_MerchantUI.Merchant.SellToClient(
                this.m_MerchantUI.ClientBag,
                this.m_RuntimeItem
            );
            
            this.EventTrade?.Invoke();
        }

        private void Sell()
        {
            this.m_MerchantUI.Merchant.BuyFromClient(
                this.m_MerchantUI.ClientBag,
                this.m_RuntimeItem
            );
            
            this.EventTrade?.Invoke();
        }
    }
}