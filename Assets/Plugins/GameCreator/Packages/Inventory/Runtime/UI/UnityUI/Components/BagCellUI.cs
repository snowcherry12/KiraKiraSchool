using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Bag Cell UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoItemUI.png")]
    
    public class BagCellUI : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler,
        ISubmitHandler,
        IDragHandler,
        IBeginDragHandler,
        IEndDragHandler
    {
        public static event Action<BagCellUI> EventHoverEnter;
        public static event Action<BagCellUI> EventHoverExit;
        public static event Action<BagCellUI> EventSelect;
        public static event Action<BagCellUI, PointerEventData> EventBeginDrag;
        public static event Action<BagCellUI, PointerEventData> EventEndDrag;
        
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            EventHoverEnter = null;
            EventHoverExit = null;
            EventSelect = null;
            EventBeginDrag = null;
            EventEndDrag = null;
        }
        
        #endif
        
        private enum EnumOnDrop
        {
            Nothing,
            Move
        }
        
        private enum EnumOnChoose
        {
            Nothing,
            Select,
            Use,
            Dismantle,
            SendToBag,
            Equip,
            Unequip,
            SplitStack
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private CanvasGroup m_TranslucentDrag;
        [SerializeField] private GameObject m_ActiveIsDrag;
        [SerializeField] private GameObject m_ActiveCanDrop;
        [SerializeField] private GameObject m_ActiveCannotDrop;
        
        [SerializeField] protected CellContentUI m_CellInfo = new CellContentUI();
        [SerializeField] protected CellMerchantUI m_CellMerchant = new CellMerchantUI();

        [SerializeField] private GameObject m_ActiveSelection;
        [SerializeField] private bool m_CanDrag = true;

        [SerializeField] private EnumOnDrop m_OnDrop = EnumOnDrop.Move;
        [SerializeField] private EnumOnChoose m_OnChoose = EnumOnChoose.Select;

        [SerializeField] private PropertyGetDecimal m_DismantleChance = GetDecimalDecimal.Create(0.5f);
        
        [SerializeField] private bool m_AllowSendEquipment;
        [SerializeField] private PropertyGetGameObject m_SendToBag = GetGameObjectPlayer.Create();

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private bool m_HasPointer;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public Cell Cell { get; private set; }
        [field: NonSerialized] public TBagUI BagUI { get; private set; }

        [field: NonSerialized] public Vector2Int Position { get; private set; }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<BagCellUI> EventRefreshUI;

        // UPDATE METHODS: ------------------------------------------------------------------------

        private void Update()
        {
            this.m_CellInfo.RefreshCooldown(
                this.BagUI != null ? this.BagUI.Bag : null, 
                this.Cell?.Item
            );
        }

        private void OnEnable()
        {
            this.m_CellMerchant.EventTrade -= this.OnTrade;
            this.m_CellMerchant.EventTrade += this.OnTrade;
            
            this.m_HasPointer = false;
            this.SetupEvents();

            if (this.m_ActiveIsDrag != null) this.m_ActiveIsDrag.SetActive(false);
            if (this.m_ActiveCanDrop != null) this.m_ActiveCanDrop.SetActive(false);
            if (this.m_ActiveCannotDrop != null) this.m_ActiveCannotDrop.SetActive(false);
        }

        private void OnDisable()
        {
            this.m_CellMerchant.EventTrade -= this.OnTrade;
            
            if (this.m_HasPointer) this.OnPointerExit(null);
            if (this.BagUI == null) return;
            
            this.BagUI.EventDragBegin -= this.OnBagDragBegin;
            this.BagUI.EventDragUpdate -= this.OnBagDragUpdate;
            this.BagUI.EventDragEnd -= this.OnBagDragEnd;
        }
        
        public void OnCreate(TBagUI bagUI)
        {
            this.BagUI = bagUI;
            this.SetupEvents();
        }

        private void SetupEvents()
        {
            if (this.BagUI == null) return;
            
            this.BagUI.EventDragBegin -= this.OnBagDragBegin;
            this.BagUI.EventDragUpdate -= this.OnBagDragUpdate;
            this.BagUI.EventDragEnd -= this.OnBagDragEnd;
            
            this.BagUI.EventDragBegin += this.OnBagDragBegin;
            this.BagUI.EventDragUpdate += this.OnBagDragUpdate;
            this.BagUI.EventDragEnd += this.OnBagDragEnd;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual void RefreshUI(int i, int j)
        {
            this.Position = new Vector2Int(i, j);
            
            this.Cell = this.BagUI.Bag.Content.GetContent(this.Position);

            bool useSingleChunk = this.BagUI.Bag.Content.CellsAsChunks == false;
            this.m_CellInfo.RefreshUI(this.BagUI.Bag, this.Position, useSingleChunk);
            this.m_CellMerchant.RefreshUI(this.BagUI, this.Position);

            this.RefreshActiveSelection();
            this.EventRefreshUI?.Invoke(this);

            Item item = this.Cell?.RootRuntimeItem?.Item;
            this.gameObject.SetActive(
                this.BagUI.FilterByParent == null ||
                item != null && item.InheritsFrom(this.BagUI.FilterByParent.ID)
            );
        }

        // PRIVATE CALLBACKS: ---------------------------------------------------------------------

        private void OnTrade()
        {
            if (this.BagUI == null) return;
            this.BagUI.RefreshUI();
        }
        
        public void OnPointerEnter(PointerEventData _)
        {
            if (this.Cell?.RootRuntimeItem == null) return;

            this.m_HasPointer = true;
            this.m_CellInfo.OnHover(this.Cell?.RootRuntimeItem);
            EventHoverEnter?.Invoke(this);
        }
        
        public void OnPointerExit(PointerEventData _)
        {
            this.m_HasPointer = false;
            EventHoverExit?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData data) => this.OnChoose(data);
        public void OnSubmit(BaseEventData data) => this.OnChoose(data);

        private void OnChoose(BaseEventData data)
        {
            if (this.Cell?.RootRuntimeItem == null) return;
            
            switch (this.m_OnChoose)
            {
                case EnumOnChoose.Nothing:
                    break;
                
                case EnumOnChoose.Select:
                    this.Select();
                    data.Use();
                    break;
                
                case EnumOnChoose.Use:
                    this.Use();
                    data.Use();
                    break;
                
                case EnumOnChoose.Dismantle:
                    float chance = (float) this.m_DismantleChance.Get(this.BagUI.Bag.Args);
                    this.Dismantle(chance);
                    data.Use();
                    break;
                
                case EnumOnChoose.SendToBag:
                    Bag sendToBag = this.m_SendToBag.Get<Bag>(this.BagUI.Bag.Args);
                    this.SendToBag(sendToBag);
                    data.Use();
                    break;

                case EnumOnChoose.Equip:
                    this.Equip();
                    data.Use();
                    break;
                
                case EnumOnChoose.Unequip:
                    this.Unequip();
                    data.Use();
                    break;
                
                case EnumOnChoose.SplitStack:
                    this.Split();
                    data.Use();
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }

            if (this.Cell?.RootRuntimeItem == null)
            {
                this.OnPointerExit(null);
            }
        }

        public void OnDrag(PointerEventData data)
        {
            this.BagUI.ReceiveEventDragUpdate(this, data);
        }

        public void OnBeginDrag(PointerEventData data)
        {
            if (!this.m_CanDrag) return;
            
            EventBeginDrag?.Invoke(this, data);
            this.BagUI.ReceiveEventDragBegin(this, data);
            data.Use();
        }
        
        public void OnEndDrag(PointerEventData data)
        {
            if (!this.m_CanDrag) return;
            if (this.Cell?.RootRuntimeItem != null)
            {
                bool successfulDrag =
                    this.OnEndDragSocket(data) ||
                    this.OnEndDragCell(data)   ||
                    this.OnEndDragEquip(data)  ||
                    this.OnEndDragToWorld(data);
            
                if (successfulDrag) data.Use();   
            }

            EventEndDrag?.Invoke(this, data);
            this.BagUI.ReceiveEventDragEnd(this, data);
        }
        
        private void OnBagDragBegin(BagCellUI dragCellUI, PointerEventData data)
        {
            bool isDragging = this.Cell == dragCellUI.Cell;
            if (this.m_ActiveIsDrag != null) this.m_ActiveIsDrag.SetActive(isDragging);
            if (this.m_TranslucentDrag != null) this.m_TranslucentDrag.alpha = isDragging ? 0.5f : 1f;
            if (this.m_ActiveCanDrop != null) this.m_ActiveCanDrop.SetActive(false);
            if (this.m_ActiveCannotDrop != null) this.m_ActiveCannotDrop.SetActive(false);
        }
        
        private void OnBagDragUpdate(BagCellUI dragCellUI, PointerEventData data)
        {
            bool isDropCell = this.BagUI.CandidateDropPositions.Contains(this.Position);
            bool canDrop = this.BagUI.CandidateDropValid;
            
            if (this.m_ActiveCanDrop != null) this.m_ActiveCanDrop.SetActive(isDropCell && canDrop);
            if (this.m_ActiveCannotDrop != null) this.m_ActiveCannotDrop.SetActive(isDropCell && !canDrop);
        }

        private void OnBagDragEnd(BagCellUI dragCellUI, PointerEventData data)
        {
            if (this.m_TranslucentDrag != null) this.m_TranslucentDrag.alpha = 1f;
            if (this.m_ActiveIsDrag != null) this.m_ActiveIsDrag.SetActive(false);
            if (this.m_ActiveCanDrop != null) this.m_ActiveCanDrop.SetActive(false);
            if (this.m_ActiveCannotDrop != null) this.m_ActiveCannotDrop.SetActive(false);
        }
        
        // DROP METHODS: --------------------------------------------------------------------------
        
        private bool OnEndDragSocket(PointerEventData data)
        {
            if (data.pointerEnter == null) return false;
            
            SocketUI targetSocketUI = data.pointerEnter.Get<SocketUI>();
            return targetSocketUI != null && targetSocketUI.OnDropCellUI(this);
        }
        
        private bool OnEndDragCell(PointerEventData data)
        {
            if (data.pointerEnter == null) return false;
            
            BagCellUI targetCellUI = data.pointerEnter.Get<BagCellUI>();
            return targetCellUI != null && targetCellUI.OnDropCellUI(this);
        }

        private bool OnEndDragEquip(PointerEventData data)
        {
            if (data.pointerEnter == null) return false;
            
            BagEquipUI equipUI = data.pointerEnter.Get<BagEquipUI>();
            return equipUI != null && equipUI.OnDropCellUI(this);
        }

        private bool OnEndDragToWorld(PointerEventData data)
        {
            Camera cam = ShortcutMainCamera.Instance.Get<Camera>();
            if (cam == null) return false;

            Ray ray = cam.ScreenPointToRay(data.position);
            if (!Physics.Raycast(ray, out RaycastHit hit) || hit.collider == null)
            {
                return false;
            }

            Trigger trigger = hit.collider.Get<Trigger>();
            if (trigger != null)
            {
                Item.LastItemDropped = this.Cell.Peek();
                CommandArgs commandArgs = new CommandArgs(
                    EventInventoryOnDropItem.COMMAND_DROP_ITEM,
                    this.BagUI.Bag.gameObject
                );
                
                trigger.OnReceiveCommand(commandArgs);
                return true;
            }

            if (this.BagUI == null || !this.BagUI.CanDropOutside) return false;

            Vector3 wearerPosition = this.BagUI.Bag.Wearer.transform.position;
            Vector3 direction = hit.point - wearerPosition;
                
            direction = Vector3.ClampMagnitude(direction, this.BagUI.MaxDropDistance);
            Vector3 point = wearerPosition + direction;
            int dropAmount = TBagUI.DropAmount switch
            {
                TBagUI.EnumDropAmount.One => 1,
                TBagUI.EnumDropAmount.Stack => this.Cell.Count,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            Item.LastItemDropped = this.Cell.Peek();
            GameObject[] instances = this.BagUI.Bag.Content.Drop(
                this.Position, 
                dropAmount, 
                point
            );
                
            return instances.Length > 0;
        }

        // INTERNAL CALLBACKS: --------------------------------------------------------------------
        
        internal bool OnDropCellUI(BagCellUI dropCellUI)
        {
            if (dropCellUI == null) return false;
            if (dropCellUI == this) return false;
            if (this.m_CellInfo.Bag == null) return false;
            if (this.m_CellInfo.Bag != dropCellUI.m_CellInfo.Bag) return false;
            
            IBagContent content = this.m_CellInfo.Bag.Content;
            
            return this.m_OnDrop switch
            {
                EnumOnDrop.Nothing => false,
                EnumOnDrop.Move => content.Move(dropCellUI.Position, this.Position, true),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Select()
        {
            this.m_CellInfo.OnSelect(this.Cell?.RootRuntimeItem);
            EventSelect?.Invoke(this);
        }
        
        public void Use()
        {
            if (this.Cell == null || this.Cell.Available) return;
            this.m_CellInfo.Bag.Content.Use(this.Position);
        }

        public void Dismantle(float chance)
        {
            if (this.Cell == null || this.Cell.Available) return;
            this.Cell.Peek().Dismantle(chance);
        }
        
        public void SendToBag(Bag bag)
        {
            if (this.Cell == null || this.Cell.Available) return;

            int times = TBagUI.TransferAmount switch
            {
                TBagUI.EnumTransferAmount.One => 1,
                TBagUI.EnumTransferAmount.Stack => this.Cell.Count,
                _ => throw new ArgumentOutOfRangeException()
            };

            for (int i = 0; i < times; ++i)
            {
                RuntimeItem runtimeItem = this.Cell.Peek();
                
                if (bag.Content.CanAdd(runtimeItem, true) == false) return;
                if (this.m_AllowSendEquipment == false)
                {
                    RuntimeItem rootRuntimeItem = this.Cell.RootRuntimeItem;
                    if (runtimeItem.Bag.Equipment.IsEquipped(rootRuntimeItem))
                    {
                        return;
                    }    
                }
                
                RuntimeItem removeRuntimeItem = this.BagUI.Bag.Content.Remove(runtimeItem);
                if (removeRuntimeItem != null) bag.Content.Add(removeRuntimeItem, true);   
            }
        }

        public void Equip()
        {
            if (this.Cell == null || this.Cell.Available) return;
            
            RuntimeItem runtimeItem = this.Cell.RootRuntimeItem;
            this.m_CellInfo.Bag.Equipment.Equip(runtimeItem);
        }
        
        public void Unequip()
        {
            if (this.Cell == null || this.Cell.Available) return;
            
            RuntimeItem runtimeItem = this.Cell.RootRuntimeItem;
            this.m_CellInfo.Bag.Equipment.Unequip(runtimeItem);
        }

        public void Drop(float randomRadius = 1f)
        {
            if (this.Cell == null || this.Cell.Available) return;
            
            RuntimeItem runtimeItem = this.Cell.Peek();
            if (!runtimeItem.Item.HasPrefab) return;
            if (!runtimeItem.Item.CanDrop) return;

            Transform target = this.BagUI.Bag.Wearer != null
                ? this.BagUI.Bag.Wearer.transform
                : this.BagUI.Bag.transform;
            
            int times = TBagUI.DropAmount switch
            {
                TBagUI.EnumDropAmount.One => 1,
                TBagUI.EnumDropAmount.Stack => this.Cell.Count,
                _ => throw new ArgumentOutOfRangeException()
            };

            for (int i = 0; i < times; ++i)
            {
                this.BagUI.Bag.Content.Drop(
                    this.Position, 1, 
                    target.position + UnityEngine.Random.insideUnitSphere * randomRadius
                );
            }
        }

        public void Destroy()
        {
            if (this.Cell == null || this.Cell.Available) return;
            
            RuntimeItem runtimeItem = this.Cell.Peek();
            this.BagUI.Bag.Content.Remove(runtimeItem);
        }

        public void Split()
        {
            if (this.Cell == null || this.Cell.Available) return;
            if (this.Cell.Count <= 1) return;
            
            if (!this.BagUI.Bag.Content.CanAddType(this.Cell.Item, false)) return;

            int splitAmount = TBagUI.SplitAmount switch
            {
                TBagUI.EnumSplitAmount.One => 1,
                TBagUI.EnumSplitAmount.Half => this.Cell.Count / 2,
                _ => throw new ArgumentOutOfRangeException()
            };

            RuntimeItem runtimeItem = this.BagUI.Bag.Content.Remove(this.Position);
            Vector2Int newPosition = this.BagUI.Bag.Content.Add(runtimeItem, false);

            if (newPosition == TBagContent.INVALID)
            {
                this.BagUI.Bag.Content.Add(runtimeItem, this.Position, true);
                return;
            }
            
            for (int i = 1; i < splitAmount; ++i)
            {
                runtimeItem = this.BagUI.Bag.Content.Remove(this.Position);
                this.BagUI.Bag.Content.Add(runtimeItem, newPosition, true);
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshActiveSelection()
        {
            if (this.m_ActiveSelection != null)
            {
                RuntimeItem runtimeItem = this.Cell is { Available: false } 
                    ? this.Cell.RootRuntimeItem 
                    : null;
                
                if (runtimeItem == null || RuntimeItem.UI_LastItemSelected == null)
                {
                    this.m_ActiveSelection.SetActive(false);
                }
                else
                {
                    int currentHash = runtimeItem.RuntimeID.Hash;
                    int selectionHash = RuntimeItem.UI_LastItemSelected.RuntimeID.Hash;
                    this.m_ActiveSelection.SetActive(currentHash == selectionHash);
                }
            }
        }
    }
}