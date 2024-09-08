using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [AddComponentMenu("Game Creator/Inventory/Bag")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoBag.png")]
    
    [DisallowMultipleComponent]
    public class Bag : MonoBehaviour
    {
        private const string ERR_NULL_SKIN = "Bag UI skin could not be found";
        private const string ERR_NO_BAG_UI = "No TBagUI component could be found";
        private const string ERR_TYPE = "Unexpected Bag type set to '{0}'. Does not match Skin";

        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeReference] private TBag m_Bag = new BagList();
        [SerializeField] private Stock m_Stock = new Stock();

        [SerializeField] private BagSkin m_SkinUI;
        [SerializeField] private PropertyGetGameObject m_Wearer = GetGameObjectSelf.Create();

        // PROPERTIES: ----------------------------------------------------------------------------

        public Args Args { get; private set; }

        public Type Type => this.m_Bag.GetType();
        
        public IBagShape Shape => this.m_Bag.Shape;
        public IBagContent Content => this.m_Bag.Content;
        public IBagEquipment Equipment => this.m_Bag.Equipment;
        public IBagCooldowns Cooldowns => this.m_Bag.Cooldowns;
        public IBagWealth Wealth => this.m_Bag.Wealth;
        
        public GameObject Wearer => this.m_Wearer.Get(this.Args);

        private GameObject SkinUI { get; set; }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChange;
        public event Action EventOpenUI;

        // INITIALIZER: ---------------------------------------------------------------------------

        private void Awake()
        {
            GameObject self = this.gameObject;
            this.Args = new Args(self, this.m_Wearer.Get(self));

            this.Shape.EventChange += () => this.EventChange?.Invoke();
            this.Content.EventChange += () => this.EventChange?.Invoke();
            this.Equipment.EventChange += (_, _) => this.EventChange?.Invoke();
            this.Wealth.EventChange += (_, _, _) => this.EventChange?.Invoke();
            
            this.m_Bag.OnAwake(this);

            int initialStockLength = this.m_Stock.StockLength;
            for (int i = 0; i < initialStockLength; ++i)
            {
                for (int j = 0; j < this.m_Stock.GetStockAmount(i); ++j)
                {
                    this.Content.AddType(this.m_Stock.GetStockItem(i), true);
                }
            }
            
            int initialWealthLength = this.m_Stock.WealthLength;
            for (int i = 0; i < initialWealthLength; ++i)
            {
                Currency currency = this.m_Stock.GetWealthCurrency(i);
                int amount = this.m_Stock.GetWealthAmount(i);
                if (currency != null) this.Wealth.Add(currency, amount);
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void OpenUI()
        {
            if (MerchantUI.IsOpen) return;
            if (TinkerUI.IsOpen) return;
            if (TBagUI.IsOpen) return;

            this.RequireSkinUI();
            if (this.SkinUI.gameObject.activeSelf) return;

            this.SkinUI.Get<TBagUI>().OpenUI(this);
            this.SkinUI.SetActive(true);
            this.EventOpenUI?.Invoke();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void RequireSkinUI()
        {
            if (this.SkinUI != null) return;
            
            if (this.m_SkinUI == null)
            {
                Debug.LogError(ERR_NULL_SKIN);
                return;
            }
            
            if (!this.m_SkinUI.HasValue)
            {
                Debug.LogError(ERR_NULL_SKIN);
                return;
            }

            if (!this.m_SkinUI.MatchType(this.Type))
            {
                Debug.LogError(string.Format(ERR_TYPE, this.Type));
                return;
            }
            
            this.SkinUI = Instantiate(this.m_SkinUI.Value);
            
            TBagUI bagUI = this.SkinUI.Get<TBagUI>();
            
            if (bagUI == null)
            {
                Debug.LogError(ERR_NO_BAG_UI);
                return;
            }
            
            bagUI.gameObject.SetActive(false);
        }
    }   
}
