using System;
using System.Runtime.InteropServices.WindowsRuntime;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [AddComponentMenu("Game Creator/Inventory/Merchant")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoMerchant.png")]
    
    [DisallowMultipleComponent]
    public class Merchant : MonoBehaviour
    {
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            EventBuyAny = null;
            EventSellAny = null;

            LastItemBought = null;
            LastItemSold = null;
        }
        
        #endif

        public static RuntimeItem LastItemBought;
        public static RuntimeItem LastItemSold;
        
        private const string ERR_NULL_SKIN = "Merchant UI skin could not be found";
        private const string ERR_NO_MERCHANT_UI = "No Merchant UI component could be found";
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private MerchantInfo m_Info = new MerchantInfo();
        
        [SerializeField] private bool m_InfiniteCurrency;
        [SerializeField] private bool m_InfiniteStock;
        [SerializeField] private bool m_AllowBuyBack;
        [SerializeField] private bool m_SellNicheType;
        [SerializeField] private Item m_SellType;

        [SerializeField] private PropertyGetDecimal m_BuyRate = GetDecimalDecimal.Create(1f);
        [SerializeField] private PropertyGetDecimal m_SellRate = GetDecimalDecimal.Create(0.5f);
        
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectSelf.Create();
        [SerializeField] private MerchantSkin m_SkinUI;

        // PROPERTIES: ----------------------------------------------------------------------------

        public string Name => this.m_Info.GetName(this.gameObject);
        public string Description => this.m_Info.GetDescription(this.gameObject);

        public Bag Bag => this.m_Bag.Get<Bag>(this.gameObject);
        public MerchantUI SkinUI { get; private set; }

        // EVENTS: --------------------------------------------------------------------------------

        public static event Action EventBuyAny;
        public static event Action EventSellAny;
        
        public event Action EventBuy;
        public event Action EventSell;
        
        public event Action EventOpenUI;
        public event Action EventCloseUI;
        
        // UI METHODS: ----------------------------------------------------------------------------

        public void OpenUI(Bag clientBag)
        {
            if (MerchantUI.IsOpen) return;
            if (TinkerUI.IsOpen) return;
            if (TBagUI.IsOpen) return;

            this.RequireMerchantUI();
            if (this.SkinUI.gameObject.activeSelf) return;

            this.SkinUI.gameObject.SetActive(true);
            bool openSuccess = this.SkinUI.OpenUI(this, clientBag);
            if (openSuccess) this.EventOpenUI?.Invoke();
        }

        public void CloseUI()
        {
            if (this.SkinUI == null) return;
            this.RequireMerchantUI();
            
            if (!this.SkinUI.gameObject.activeSelf) return;

            this.SkinUI.gameObject.SetActive(false);
            this.EventCloseUI?.Invoke();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        /// <summary>
        /// Returns true if the Merchant can buy an item from the client
        /// </summary>
        /// <param name="clientBag"></param>
        /// <param name="runtimeItem"></param>
        /// <returns></returns>
        public bool CanBuyFromClient(Bag clientBag, RuntimeItem runtimeItem)
        {
            if (runtimeItem is { CanSellToMerchant: false }) return false;
            Bag merchantBag = this.Bag;
            
            if (merchantBag == null) return false;
            if (clientBag == null) return false;

            if (!clientBag.Content.Contains(runtimeItem)) return false;
            if (clientBag.Equipment.IsEquipped(runtimeItem)) return false;
            
            if (merchantBag.Content.Contains(runtimeItem)) return false;

            Item item = runtimeItem.Item;
            if (this.m_SellNicheType)
            {
                bool notBelong = this.m_SellType != null && !item.InheritsFrom(this.m_SellType.ID); 
                if (notBelong) return false;
            }

            if (!this.m_InfiniteCurrency)
            {
                int price = this.GetSellPrice(runtimeItem, clientBag);
                if (merchantBag.Wealth.Get(item.Price.Currency) < price) return false;
            }

            return merchantBag.Content.CanAdd(runtimeItem, true);
        }

        /// <summary>
        /// Returns true if the Merchant can sell an item to the client
        /// </summary>
        /// <param name="clientBag"></param>
        /// <param name="runtimeItem"></param>
        /// <returns></returns>
        public bool CanSellToClient(Bag clientBag, RuntimeItem runtimeItem)
        {
            if (runtimeItem is { CanBuyFromMerchant: false }) return false;
            Bag merchantBag = this.Bag;
            
            if (merchantBag == null) return false;
            if (clientBag == null) return false;

            if (clientBag.Content.Contains(runtimeItem)) return false;
            if (!merchantBag.Content.Contains(runtimeItem)) return false;
            if (merchantBag.Equipment.IsEquipped(runtimeItem)) return false;
            
            int price = this.GetBuyPrice(runtimeItem, clientBag);
            if (clientBag.Wealth.Get(runtimeItem.Currency.ID) < price) return false;
            
            return clientBag.Content.CanAdd(runtimeItem, true);
        }

        /// <summary>
        /// The Merchant purchases an item from the client
        /// </summary>
        /// <param name="clientBag"></param>
        /// <param name="runtimeItem"></param>
        /// <returns></returns>
        public bool BuyFromClient(Bag clientBag, RuntimeItem runtimeItem)
        {
            if (!this.CanBuyFromClient(clientBag, runtimeItem)) return false;

            runtimeItem = clientBag.Content.Remove(runtimeItem);
            if (runtimeItem == null) return false;
            
            Bag merchantBag = this.Bag;

            IdString currencyID = runtimeItem.Currency.ID;
            int price = this.GetSellPrice(runtimeItem, clientBag);
            
            if (!this.m_InfiniteCurrency) merchantBag.Wealth.Subtract(currencyID, price);
            clientBag.Wealth.Add(currencyID, price);

            if (!this.m_AllowBuyBack || merchantBag.Content.Add(runtimeItem, true) != TBagContent.INVALID)
            {
                LastItemSold = runtimeItem;
                this.EventSell?.Invoke();
                EventSellAny?.Invoke();
                return true;
            }

            return false;
        }

        /// <summary>
        /// The Merchant sells an item to the client
        /// </summary>
        /// <param name="clientBag"></param>
        /// <param name="runtimeItem"></param>
        /// <returns></returns>
        public bool SellToClient(Bag clientBag, RuntimeItem runtimeItem)
        {
            if (!this.CanSellToClient(clientBag, runtimeItem)) return false;

            Bag merchantBag = this.Bag;

            Currency currency = runtimeItem.Item.Price.Currency;
            int price = this.GetBuyPrice(runtimeItem, clientBag);

            if (currency != null)
            {
                clientBag.Wealth.Subtract(currency, price);
                if (!this.m_InfiniteCurrency) merchantBag.Wealth.Add(currency, price);
            }
            
            if (this.m_InfiniteStock)
            {
                RuntimeItem runtimeItemAdded = clientBag.Content.AddType(runtimeItem.Item, true); 
                if (runtimeItemAdded != null)
                {
                    LastItemBought = runtimeItemAdded;
                    this.EventBuy?.Invoke();
                    EventBuyAny?.Invoke();
                    return true;
                }
            }

            merchantBag.Content.Remove(runtimeItem);
            if (clientBag.Content.Add(runtimeItem, true) != TBagContent.INVALID)
            {
                LastItemBought = runtimeItem;
                this.EventBuy?.Invoke();
                EventBuyAny?.Invoke();
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// The price an item is sold to the client
        /// </summary>
        /// <param name="runtimeItem"></param>
        /// <param name="clientBag"></param>
        /// <returns></returns>
        public int GetSellPrice(RuntimeItem runtimeItem, Bag clientBag)
        {
            if (runtimeItem == null) return 0;
            Bag merchantBag = this.Bag;
            
            if (merchantBag == null) return 0;
            if (clientBag == null) return 0;

            Args args = new Args(merchantBag, clientBag);
            double sellRate = this.m_SellRate.Get(args);
            
            return (int) Math.Floor(runtimeItem.Price * sellRate);
        }
        
        /// <summary>
        /// The price an item is purchased from the client
        /// </summary>
        /// <param name="runtimeItem"></param>
        /// <param name="clientBag"></param>
        /// <returns></returns>
        public int GetBuyPrice(RuntimeItem runtimeItem, Bag clientBag)
        {
            if (runtimeItem == null) return 0;
            Bag merchantBag = this.Bag;
            
            if (merchantBag == null) return 0;
            if (clientBag == null) return 0;

            Args args = new Args(merchantBag, clientBag);
            double buyRate = this.m_BuyRate.Get(args);

            return (int) Math.Floor(runtimeItem.Price * buyRate);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RequireMerchantUI()
        {
            if (this.SkinUI != null) return;
            if (this.m_SkinUI == null) throw new NullReferenceException(ERR_NULL_SKIN);
            if (!this.m_SkinUI.HasValue) throw new NullReferenceException(ERR_NULL_SKIN);

            GameObject gameObjectMerchantUI = Instantiate(this.m_SkinUI.Value);
            this.SkinUI = gameObjectMerchantUI.Get<MerchantUI>();
            
            if (this.SkinUI == null) throw new NullReferenceException(ERR_NO_MERCHANT_UI);
            this.SkinUI.gameObject.SetActive(false);
        }
    }
}