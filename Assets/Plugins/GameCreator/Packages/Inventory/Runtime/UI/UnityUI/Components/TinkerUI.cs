using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Tinker UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoTinkerUI.png")]
    
    public class TinkerUI : MonoBehaviour
    {
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode()
        {
            IsOpen = false;
            LastBagTinkerInput = null;
            LastBagTinkerOutput = null;
            LastTinkerUIOpened = null;
            EventOpen = null;
            EventClose = null;
        }
        
        #endif
        
        public static Bag LastBagTinkerInput;
        public static Bag LastBagTinkerOutput;
        public static GameObject LastTinkerUIOpened;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private TinkerCraftingUI m_CraftingUI = new TinkerCraftingUI();
        [SerializeField] private TinkerDismantlingUI m_DismantlingUI = new TinkerDismantlingUI();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public static bool IsOpen { get; private set; }

        [field: NonSerialized] public Bag InputBag  { get; private set; }
        [field: NonSerialized] public Bag OutputBag { get; private set; }
        
        [field: NonSerialized] public TTinkerItemUI Selection { get; private set; }

        // EVENTS: --------------------------------------------------------------------------------

        public static event Action EventOpen;
        public static event Action EventClose;
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        private void OnEnable()
        {
            if (this.InputBag != null)
            {
                this.InputBag.EventChange -= this.RefreshUI;
                this.InputBag.EventChange += this.RefreshUI;
            }

            if (this.OutputBag != null)
            {
                this.OutputBag.EventChange -= this.RefreshUI;
                this.OutputBag.EventChange += this.RefreshUI;   
            }

            if (this.InputBag != null && this.OutputBag != null)
            {
                this.RefreshUI();
            }
            
            IsOpen = true;
            EventOpen?.Invoke();
        }

        private void OnDisable()
        {
            if (this.InputBag != null) this.InputBag.EventChange -= this.RefreshUI;
            if (this.OutputBag != null) this.OutputBag.EventChange -= this.RefreshUI;

            IsOpen = false;
            EventClose?.Invoke();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void OpenUI(Bag inputBag, Bag outputBag, Item filterItem = null)
        {
            if (inputBag == null) return;
            if (outputBag == null) return;

            LastBagTinkerInput = inputBag;
            LastBagTinkerOutput = outputBag;
            LastTinkerUIOpened = this.gameObject;
            
            this.InputBag = inputBag;
            this.OutputBag = outputBag;
            
            this.InputBag.EventChange -= this.RefreshUI;
            this.InputBag.EventChange += this.RefreshUI;

            this.OutputBag.EventChange -= this.RefreshUI;
            this.OutputBag.EventChange += this.RefreshUI;

            this.m_CraftingUI.FilterByParent = filterItem;
            this.m_DismantlingUI.FilterByParent = filterItem;

            this.Selection = null;
            this.RefreshUI();
        }

        public void RefreshUI()
        {
            this.m_CraftingUI.RefreshUI(this);
            this.m_DismantlingUI.RefreshUI(this);
        }

        public void OnSelect(TTinkerItemUI tinkerItemUI)
        {
            this.Selection = tinkerItemUI;
            this.RefreshUI();
        }
    }
}