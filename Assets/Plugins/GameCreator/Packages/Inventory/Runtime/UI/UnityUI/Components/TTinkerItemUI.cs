using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoTooltipUI.png")]
    
    public abstract class TTinkerItemUI : MonoBehaviour
    {
        [SerializeField] private Button m_ButtonSelect;
        [SerializeField] private GameObject m_ActiveIfSelected;
        
        [SerializeField] private PropertyGetDecimal m_Duration = new PropertyGetDecimal(0f);

        [SerializeField] private RuntimeItemUI m_ItemUI = new RuntimeItemUI();
        [SerializeField] private GameObject m_ActiveIfCanTinker;
        [SerializeField] private Button m_ButtonTinker;

        [SerializeField] private TextReference m_AmountInInputBag = new TextReference();
        [SerializeField] private TextReference m_AmountInOutputBag = new TextReference();
        
        [SerializeField] private GameObject m_ActiveIfInProgress;
        [SerializeField] private Image m_Progress;

        [SerializeField] private GameObject m_PrefabIngredientUI;
        [SerializeField] private RectTransform m_IngredientsContent;

        [SerializeField] protected InstructionList m_OnStart = new InstructionList();
        [SerializeField] protected InstructionList m_OnComplete = new InstructionList();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public Bag InputBag  { get; private set; }
        [field: NonSerialized] public Bag OutputBag { get; private set; }
        [field: NonSerialized] public RuntimeItem RuntimeItem { get; private set; }

        protected Args Args => new Args(
            this.gameObject,
            this.InputBag.gameObject
        );
        
        [field: NonSerialized] public bool IsRunning { get; private set; }
        [field: NonSerialized] private bool IsCancelled { get; set; }
        
        [field: NonSerialized] private TinkerUI TinkerUI { get; set; }
        
        protected abstract bool CanTinker { get; }
        protected abstract bool EnoughIngredients { get; }
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            if (this.m_ButtonTinker != null)
            {
                this.m_ButtonTinker.onClick.RemoveListener(this.Tinker);
                this.m_ButtonTinker.onClick.AddListener(this.Tinker);
            }

            if (this.m_ButtonSelect != null)
            {
                this.m_ButtonSelect.onClick.RemoveListener(this.Select);
                this.m_ButtonSelect.onClick.AddListener(this.Select);
            }
        }

        private void OnDisable()
        {
            this.IsCancelled = true;
            
            if (this.m_ButtonTinker != null)
            {
                this.m_ButtonTinker.onClick.RemoveListener(this.Tinker);
            }
            
            if (this.m_ButtonSelect != null)
            {
                this.m_ButtonSelect.onClick.RemoveListener(this.Select);
            }
        }

        private void Update()
        {
            this.m_ItemUI.RefreshCooldown(
                this.InputBag, 
                this.RuntimeItem?.Item
            );
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void RefreshUI(TinkerUI tinkerUI, RuntimeItem runtimeItem)
        {
            this.TinkerUI = tinkerUI;
            
            this.InputBag = this.TinkerUI.InputBag;
            this.OutputBag = this.TinkerUI.OutputBag;
            
            this.RuntimeItem = runtimeItem;
            
            if (this.RuntimeItem == null) return;
            if (this.InputBag == null) return;

            this.m_ItemUI.RefreshUI(this.InputBag, this.RuntimeItem, true, true);

            if (this.m_ActiveIfSelected != null)
            {
                bool selected = tinkerUI.Selection != null && tinkerUI.Selection.RuntimeItem == this.RuntimeItem;
                this.m_ActiveIfSelected.SetActive(selected);
            }
            
            if (this.m_ActiveIfCanTinker != null)
            {
                this.m_ActiveIfCanTinker.SetActive(this.CanTinker);
            }

            if (this.m_ButtonTinker != null)
            {
                this.m_ButtonTinker.interactable = this.CanTinker && this.EnoughIngredients;
            }

            this.m_AmountInInputBag.Text = this.InputBag.Content.CountType(this.RuntimeItem.Item).ToString();
            this.m_AmountInOutputBag.Text = this.OutputBag.Content.CountType(this.RuntimeItem.Item).ToString();

            if (this.m_ActiveIfInProgress != null)
            {
                this.m_ActiveIfInProgress.SetActive(this.IsRunning);
            }

            if (this.m_PrefabIngredientUI != null && this.m_IngredientsContent != null)
            {
                int numIngredients = this.RuntimeItem.Item.Crafting.Ingredients.Length;
                int numChildren = this.m_IngredientsContent.childCount;

                int numCreate = numIngredients - numChildren;
                int numDelete = numChildren - numIngredients;

                for (int i = numCreate - 1; i >= 0; --i) this.CreateIngredient();
                for (int i = numDelete - 1; i >= 0; --i) this.DeleteIngredient(numIngredients + i);
            
                for (int i = 0; i < numIngredients; ++i)
                {
                    Transform child = this.m_IngredientsContent.GetChild(i);
                    TTinkerIngredientUI ingredientUI = child.Get<TTinkerIngredientUI>();
                
                    if (ingredientUI == null) continue;
                    ingredientUI.RefreshUI(this.InputBag, this.RuntimeItem.Item.Crafting.Ingredients[i]);
                }
            }
        }
        
        public void Select()
        {
            if (this.TinkerUI == null) return;
            this.TinkerUI.OnSelect(this);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void CreateIngredient()
        {
            Instantiate(this.m_PrefabIngredientUI, this.m_IngredientsContent);
        }
        
        private void DeleteIngredient(int index)
        {
            Transform child = this.m_IngredientsContent.GetChild(index);
            Destroy(child.gameObject);
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected async Task<bool> WaitForTime()
        {
            float duration = (float) this.m_Duration.Get(this.Args);
            float startTime = Time.unscaledTime;
            
            this.IsRunning = true;
            this.IsCancelled = false;
            
            if (this.m_ButtonTinker != null) this.m_ButtonTinker.interactable = false;
            
            while (Time.unscaledTime < startTime + duration)
            {
                if (AsyncManager.ExitRequest || !this.isActiveAndEnabled || this.IsCancelled)
                {
                    this.IsRunning = false;
                    return false;
                }

                float progress = (Time.unscaledTime - startTime) / duration;
                
                if (this.m_Progress != null)
                {
                    this.m_Progress.fillAmount = Easing.Lerp(0f, 1f, progress);
                }
                
                if (this.m_ActiveIfInProgress != null)
                {
                    this.m_ActiveIfInProgress.SetActive(true);
                }
                
                await Task.Yield();
            }

            this.IsRunning = false;
            
            if (this.m_ActiveIfInProgress != null)
            {
                this.m_ActiveIfInProgress.SetActive(false);
            }

            if (this.m_ButtonTinker != null)
            {
                this.m_ButtonTinker.interactable = this.CanTinker && this.EnoughIngredients;
            }
            
            return true;
        }

        protected abstract void Tinker();
    }
}