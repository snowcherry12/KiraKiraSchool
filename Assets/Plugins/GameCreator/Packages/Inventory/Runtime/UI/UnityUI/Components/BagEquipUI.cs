using System;
using System.Collections;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Bag Equip UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoEquipmentUI.png")]
    
    public class BagEquipUI : MonoBehaviour,
        IPointerClickHandler,
        ISubmitHandler
    {
        private enum EnumOnClick
        {
            Nothing,
            Unequip,
            Use,
            Dismantle
        }
        
        private enum EnumOnSubmit
        {
            Nothing,
            Unequip,
            Use,
            Dismantle
        }

        private enum EnumOnDrop
        {
            Nothing,
            Equip
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        [SerializeField] private EquipmentIndex m_EquipmentIndex = new EquipmentIndex();

        [SerializeField] private ItemUI m_BaseUI = new ItemUI();
        [SerializeField] private CellContentUI m_EquippedUI = new CellContentUI();

        [SerializeField] private EnumOnSubmit m_OnSubmit = EnumOnSubmit.Unequip;
        [SerializeField] private EnumOnClick m_OnClick = EnumOnClick.Unequip;
        [SerializeField] private EnumOnDrop m_OnDrop = EnumOnDrop.Equip;

        [SerializeField] private InputPropertyButton m_InputUse = InputButtonNone.Create();

        [SerializeField] private GameObject m_ActiveNotCooldown;
        [SerializeField] private Image m_CooldownProgress;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] private Bag Bag { get; set; }
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
        {
            this.m_InputUse.OnStartup();
            this.m_InputUse.RegisterPerform(this.OnActionUse);
        }

        private void OnDestroy()
        {
            this.m_InputUse.ForgetPerform(this.OnActionUse);
            this.m_InputUse.OnDispose();
        }

        private void OnEnable()
        {
            StartCoroutine(this.DeferredOnEnable());
        }
        
        private IEnumerator DeferredOnEnable()
        {
            yield return null;
            
            if (this.Bag != null) this.Bag.EventChange -= this.RefreshUI;

            this.Bag = this.m_Bag.Get<Bag>(this.gameObject);
            if (this.Bag != null)
            {
                this.Bag.EventChange -= this.RefreshUI;
                this.Bag.EventChange += this.RefreshUI;
            }
            
            this.RefreshUI();
        }
        
        private void OnDisable()
        {
            this.Bag = this.m_Bag.Get<Bag>(this.gameObject);
            if (this.Bag != null)
            {
                this.Bag.EventChange -= this.RefreshUI;
            }
        }
        
        private void Update()
        {
            this.m_InputUse.OnUpdate();
            
            IdString runtimeItemID = this.Bag != null 
                ? this.Bag.Equipment.GetSlotRootRuntimeItemID(this.m_EquipmentIndex.Index)
                : IdString.EMPTY;
            
            RuntimeItem runtimeItem = this.Bag != null ? this.Bag.Content.GetRuntimeItem(runtimeItemID) : null;
            
            this.m_EquippedUI.RefreshCooldown(
                this.Bag, 
                runtimeItem?.Item
            );
        }

        // CALLBACKS: -----------------------------------------------------------------------------
        
        public void OnPointerClick(PointerEventData data)
        {
            switch (this.m_OnClick)
            {
                case EnumOnClick.Nothing: break;
                case EnumOnClick.Unequip: this.Unequip(); break;
                case EnumOnClick.Use: this.Use(); break;
                case EnumOnClick.Dismantle: this.Dismantle(1f); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        public void OnSubmit(BaseEventData data)
        {
            switch (this.m_OnSubmit)
            {
                case EnumOnSubmit.Nothing: break;
                case EnumOnSubmit.Unequip: this.Unequip(); break;
                case EnumOnSubmit.Use: this.Use(); break;
                case EnumOnSubmit.Dismantle: this.Dismantle(1f); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnActionUse()
        {
            this.Use();
        }
        
        // INTERNAL CALLBACKS: --------------------------------------------------------------------

        internal bool OnDropCellUI(BagCellUI dropCellUI)
        {
            if (dropCellUI == null) return false;

            return this.m_OnDrop switch
            {
                EnumOnDrop.Nothing => false,
                EnumOnDrop.Equip => this.Equip(dropCellUI),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public void RefreshUI()
        {
            Item baseItem = null;
            Vector2Int position = new Vector2Int(-1, -1);

            if (this.Bag != null)
            {
                IBagEquipment equipment = this.Bag.Equipment;
                
                IdString baseID = equipment.GetSlotBaseID(this.m_EquipmentIndex.Index);
                IdString runtimeItemID = equipment.GetSlotRootRuntimeItemID(this.m_EquipmentIndex.Index);
             
                baseItem = Settings.From<InventoryRepository>().Items.Get(baseID);
                position = this.Bag.Content.FindPosition(runtimeItemID);
            }

            this.m_BaseUI.RefreshUI(this.Bag, baseItem, true);
            this.m_EquippedUI.RefreshUI(this.Bag, position, true);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Equip(BagCellUI bagCellUI)
        {
            if (this.Bag == null) return false;
            if (bagCellUI.Cell.Available) return false;

            RuntimeItem runtimeItem = bagCellUI.Cell.RootRuntimeItem;
            IBagEquipment equipment = this.Bag.Equipment;

            if (this.Bag != runtimeItem.Bag) return false;
            _ = equipment.EquipToIndex(runtimeItem, this.m_EquipmentIndex.Index);
            return true;
        }
        
        public RuntimeItem Unequip()
        {
            if (this.Bag == null) return null;

            IBagContent content = this.Bag.Content;
            IBagEquipment equipment = this.Bag.Equipment;
            
            IdString runtimeItemID = equipment.GetSlotRootRuntimeItemID(this.m_EquipmentIndex.Index);
            if (string.IsNullOrEmpty(runtimeItemID.String)) return null;

            RuntimeItem rootRuntimeItem = content.GetRuntimeItem(runtimeItemID);
            _ = equipment.Unequip(rootRuntimeItem);
            return rootRuntimeItem;
        }
        
        public void Use()
        {
            if (this.Bag == null) return;
            
            IBagContent content = this.Bag.Content;
            IBagEquipment equipment = this.Bag.Equipment;

            IdString runtimeItemID = equipment.GetSlotRootRuntimeItemID(this.m_EquipmentIndex.Index);
            if (string.IsNullOrEmpty(runtimeItemID.String)) return;

            Vector2Int position = content.FindPosition(runtimeItemID);
            this.Bag.Content.Use(position);
        }
        
        public void Dismantle(float chance)
        {
            if (this.Bag == null) return;
            
            IBagContent content = this.Bag.Content;
            IBagEquipment equipment = this.Bag.Equipment;
            
            IdString runtimeItemID = equipment.GetSlotRootRuntimeItemID(this.m_EquipmentIndex.Index);
            if (string.IsNullOrEmpty(runtimeItemID.String)) return;

            Vector2Int position = content.FindPosition(runtimeItemID);
            Cell cell = content.GetContent(position);
            
            if (cell == null || cell.Available) return;
            cell.Peek().Dismantle(chance);
        }
    }
}