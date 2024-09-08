using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoBagUI.png")]
    public abstract class TBagUI : MonoBehaviour
    {
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode()
        {
            IsOpen = false;
            
            LastBagOpened = null;
            LastBagUIOpened = null;
            
            TransferAmount = EnumTransferAmount.One;
            DropAmount = EnumDropAmount.One;
            SplitAmount = EnumSplitAmount.Half;
            
            EventOpen = null;
            EventClose = null;
        }
        
        #endif
        
        public enum EnumDropAmount
        {
            One,
            Stack
        }
        
        public enum EnumTransferAmount
        {
            One,
            Stack
        }
        
        public enum EnumSplitAmount
        {
            One,
            Half
        }
        
        public static Bag LastBagOpened;
        public static TBagUI LastBagUIOpened;

        public static EnumTransferAmount TransferAmount { get; set; } = EnumTransferAmount.One;
        public static EnumDropAmount DropAmount { get; set; } = EnumDropAmount.One;
        public static EnumSplitAmount SplitAmount { get; set; } = EnumSplitAmount.Half;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_DefaultBag = GetGameObjectNone.Create();
        
        [SerializeField] private GameObject m_PrefabCell;
        [SerializeField] private bool m_CanDropOutside = true;
        [SerializeField] private float m_MaxDropDistance = 1f;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public static bool IsOpen { get; private set; }

        [field: NonSerialized] public Bag Bag { get; private set; }
        [field: NonSerialized] public MerchantUI MerchantUI { get; private set; }
        
        protected GameObject PrefabCell => this.m_PrefabCell;
        protected abstract RectTransform Content { get; }
        
        public abstract Type ExpectedBagType { get; }

        [field: NonSerialized] internal List<Vector2Int> CandidateDropPositions { get; } = new List<Vector2Int>();
        [field: NonSerialized] internal bool CandidateDropValid { get; private set; }
        
        public bool CanDropOutside
        {
            get => this.m_CanDropOutside;
            set => this.m_CanDropOutside = value;
        }

        public float MaxDropDistance
        {
            get => this.m_MaxDropDistance;
            set => this.m_MaxDropDistance = value;
        }

        public virtual Item FilterByParent
        {
            get => null;
            set => this.RefreshUI();
        }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventRefreshUI;

        public static event Action EventOpen;
        public static event Action EventClose;

        public event Action<BagCellUI, PointerEventData> EventDragBegin;
        public event Action<BagCellUI, PointerEventData> EventDragUpdate;
        public event Action<BagCellUI, PointerEventData> EventDragEnd;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        protected virtual void Awake()
        {
            Bag bag = this.m_DefaultBag.Get<Bag>(this.gameObject);
            if (bag == null) return;

            this.Bag = bag;
        }

        protected virtual void OnEnable()
        {
            IsOpen = true;

            if (this.Bag != null)
            {
                this.Bag.EventChange -= this.RefreshUI;
                this.Bag.EventChange += this.RefreshUI;

                BagCellUI.EventSelect -= this.OnSelectCell;
                BagCellUI.EventSelect += this.OnSelectCell;

                this.RefreshUI();
            }
            
            EventOpen?.Invoke();
        }
        
        protected virtual void OnDisable()
        {
            if (this.Bag != null) this.Bag.EventChange -= this.RefreshUI;
            BagCellUI.EventSelect -= this.OnSelectCell;
            
            IsOpen = false;
            EventClose?.Invoke();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void OpenUI(Bag bag, MerchantUI merchantUI = null)
        {
            LastBagOpened = bag;
            LastBagUIOpened = this;
            this.MerchantUI = merchantUI;
            
            this.ChangeBag(bag, true);
            this.SelectFirst();
        }

        public virtual void RefreshUI()
        {
            this.EventRefreshUI?.Invoke();
        }

        public void ChangeBag(Bag bag, bool forceListenEvents)
        {
            if (this.Bag != null) this.Bag.EventChange -= this.RefreshUI;
            
            this.Bag = bag;
            
            if (this.Bag == null) return;
            if (!forceListenEvents && !this.isActiveAndEnabled) return;
            
            this.Bag.EventChange -= this.RefreshUI;
            this.Bag.EventChange += this.RefreshUI;
            
            BagCellUI.EventSelect -= this.OnSelectCell;
            BagCellUI.EventSelect += this.OnSelectCell;
            
            this.RefreshUI();
        }
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal void ReceiveEventDragBegin(BagCellUI dragCellUI, PointerEventData data)
        {
            this.UpdateCandidateDrop(dragCellUI, data);
            this.EventDragBegin?.Invoke(dragCellUI, data);
        }
        
        internal void ReceiveEventDragUpdate(BagCellUI dragCellUI, PointerEventData data)
        {
            this.UpdateCandidateDrop(dragCellUI, data);
            this.EventDragUpdate?.Invoke(dragCellUI, data);
        }

        private void UpdateCandidateDrop(BagCellUI dragCellUI, PointerEventData data)
        {
            this.CandidateDropPositions.Clear();
            this.CandidateDropValid = true;
            
            if (dragCellUI == null || dragCellUI.Cell == null) return;
            
            if (dragCellUI.Cell.Available) return;
            if (data.pointerEnter == null) return;
            
            Bag dragBag = dragCellUI.Cell.RootRuntimeItem.Bag;
            if (dragBag == null) return;

            Vector2Int dragPointPosition = dragCellUI.Position;
            Vector2Int dragRootPosition = dragBag.Content.FindStartPosition(dragPointPosition);
            Vector2Int dragOffset = dragPointPosition - dragRootPosition;
            
            BagCellUI belowCellUI = data.pointerEnter.Get<BagCellUI>();
            if (belowCellUI == null) return;

            for (int i = 0; i < dragCellUI.Cell.Item.Shape.Width; ++i)
            {
                for (int j = 0; j < dragCellUI.Cell.Item.Shape.Height; ++j)
                {
                    Vector2Int position = new Vector2Int(
                        belowCellUI.Position.x - dragOffset.x + i,
                        belowCellUI.Position.y - dragOffset.y + j
                    );
                    
                    this.CandidateDropPositions.Add(position);
                }
            }
            
            if (dragBag.Content.CanMove(dragCellUI.Position, belowCellUI.Position, true))
            {
                this.CandidateDropValid = true;
                return;
            }
            
            RectInt bagBounds = new RectInt(
                Vector2Int.zero,
                new Vector2Int(Bag.Shape.MaxWidth, Bag.Shape.MaxHeight)
            );
            
            for (int i = 0; i < dragCellUI.Cell.Item.Shape.Width; ++i)
            {
                for (int j = 0; j < dragCellUI.Cell.Item.Shape.Height; ++j)
                {
                    Vector2Int position = new Vector2Int(
                        belowCellUI.Position.x - dragOffset.x + i,
                        belowCellUI.Position.y - dragOffset.y + j
                    );
                    
                    if (bagBounds.Contains(position))
                    {
                        Cell cell = dragBag.Content.GetContent(position);
                    
                        if (cell == null) continue;
                        this.CandidateDropValid &= cell.Available || cell == dragCellUI.Cell;
                    }

                    this.CandidateDropValid = false;
                }
            }
        }

        internal void ReceiveEventDragEnd(BagCellUI dragCellUI, PointerEventData data)
        {
            this.CandidateDropPositions.Clear();
            this.EventDragEnd?.Invoke(dragCellUI, data);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void OnSelectCell(BagCellUI cellUI)
        {
            this.RefreshUI();
        }

        private void SelectFirst()
        {
            if (this.Content.childCount <= 0) return;
            
            BagCellUI firstItem = this.Content.GetChild(0).Get<BagCellUI>();
            if (firstItem == null) return;
            
            firstItem.Select();
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(firstItem.gameObject);
            }
        }
    }
}