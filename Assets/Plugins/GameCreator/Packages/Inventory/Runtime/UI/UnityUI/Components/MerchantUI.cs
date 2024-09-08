using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Merchant UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoMerchantUI.png")]
    
    public class MerchantUI : MonoBehaviour
    {
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode()
        {
            IsOpen = false;
            LastBagMerchant = null;
            LastBagClient = null;
            LastMerchantUIOpened = null;
            EventOpen = null;
            EventClose = null;
        }
        
        #endif
        
        public static Bag LastBagMerchant;
        public static Bag LastBagClient;
        public static GameObject LastMerchantUIOpened;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private TBagUI m_MerchantBagUI;
        [SerializeField] private TBagUI m_ClientBagUI;

        [SerializeField] private InstructionList m_OnBuy = new InstructionList();
        [SerializeField] private InstructionList m_OnSell = new InstructionList();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public static bool IsOpen { get; private set; }
        
        [field: NonSerialized] public Merchant Merchant { get; private set; }
        [field: NonSerialized] public Bag MerchantBag { get; set; }
        [field: NonSerialized] public Bag ClientBag { get; set; }
        
        // EVENTS. --------------------------------------------------------------------------------

        public static event Action EventOpen;
        public static event Action EventClose;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        public bool OpenUI(Merchant merchant, Bag clientBag)
        {
            if (merchant == null) return false;
            if (clientBag == null) return false;

            LastBagMerchant = merchant.Bag;
            LastBagClient = clientBag;
            LastMerchantUIOpened = this.gameObject;
            
            this.Merchant = merchant;
            this.MerchantBag = merchant.Bag;
            this.ClientBag = clientBag;
            
            this.m_MerchantBagUI.OpenUI(this.MerchantBag, this);
            this.m_ClientBagUI.OpenUI(this.ClientBag, this);

            this.RefreshUI();
            return true;
        }

        private void OnEnable()
        {
            IsOpen = true;
            
            if (this.ClientBag == null) return;
            if (this.MerchantBag == null) return;

            this.RefreshUI();
            EventOpen?.Invoke();
        }

        private void OnDisable()
        {
            IsOpen = false;
            EventClose?.Invoke();

            if (this.Merchant == null) return;
            
            this.Merchant.EventBuy -= this.OnBuy;
            this.Merchant.EventSell -= this.OnSell;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshUI()
        {
            this.m_MerchantBagUI.RefreshUI();
            this.m_ClientBagUI.RefreshUI();

            if (this.Merchant == null) return;
            
            this.Merchant.EventBuy -= this.OnBuy;
            this.Merchant.EventSell -= this.OnSell;
            
            this.Merchant.EventBuy += this.OnBuy;
            this.Merchant.EventSell += this.OnSell;
        }

        private void OnBuy()
        {
            Args args = new Args(this.Merchant, this.ClientBag);

            try
            {
                _ = this.m_OnBuy.Run(args);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }

        private void OnSell()
        {
            Args args = new Args(this.Merchant, this.ClientBag);

            try
            {
                _ = this.m_OnSell.Run(args);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }
    }
}