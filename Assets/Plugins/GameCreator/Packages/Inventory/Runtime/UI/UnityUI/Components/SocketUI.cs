using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Inventory/Socket UI")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoSocketUI.png")]
    
    public class SocketUI : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler,
        ISubmitHandler,
        ISelectHandler
    {
        public static event Action<SocketUI> EventHoverEnter;
        public static event Action<SocketUI> EventHoverExit;
        
        public static event Action<SocketUI> EventSocketAttach;
        public static event Action<SocketUI> EventSocketDetach;
        
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            EventHoverEnter = null;
            EventHoverExit = null;
            EventSocketAttach = null;
            EventSocketDetach = null;
        }
        
        #endif
        
        private enum EnumOnDrop
        {
            Nothing,
            AttachWhenEmpty
        }

        private enum EnumOnSubmit
        {
            Nothing,
            RemoveToBag,
            DestroyAttachment
        }
        
        private enum EnumOnClick
        {
            Nothing,
            RemoveToBag,
            DestroyAttachment
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] protected RuntimeItemUI m_ItemUI = new RuntimeItemUI();

        [SerializeField] private EnumOnDrop m_OnDrop = EnumOnDrop.AttachWhenEmpty;
        [SerializeField] private EnumOnSubmit m_OnSubmit = EnumOnSubmit.RemoveToBag;
        [SerializeField] private EnumOnClick m_OnClick = EnumOnClick.RemoveToBag;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private RuntimeItem m_RuntimeItem;
        [NonSerialized] private RuntimeSocket m_RuntimeSocket;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public RuntimeSocket RuntimeSocket => this.m_RuntimeSocket;

        // UPDATE METHODS: ------------------------------------------------------------------------

        private void Update()
        {
            this.m_ItemUI.RefreshCooldown(
                this.m_RuntimeItem?.Bag, 
                this.m_RuntimeItem?.Item
            );
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void RefreshUI(Bag bag, RuntimeItem runtimeItem, RuntimeSocket runtimeSocket)
        {
            this.m_RuntimeItem = runtimeItem;
            this.m_RuntimeSocket = runtimeSocket;
            
            this.m_ItemUI.RefreshUI(bag, runtimeSocket.Attachment, true, true);
        }

        // PRIVATE CALLBACKS: ---------------------------------------------------------------------

        public void OnPointerEnter(PointerEventData data)
        {
            if (data.dragging) return;
            if (this.m_RuntimeSocket == null) return;
            
            this.m_ItemUI.OnHover(this.m_RuntimeSocket?.Attachment);
            RuntimeItem.UI_LastSocketHovered = this.m_RuntimeSocket;
            
            EventHoverEnter?.Invoke(this);
        }
        
        public void OnPointerExit(PointerEventData data)
        {
            if (data.dragging) return;
            EventHoverExit?.Invoke(this);
        }
        
        public void OnPointerClick(PointerEventData data)
        {
            if (this.m_OnClick == EnumOnClick.Nothing) return;
            this.RemoveAttachment(this.m_OnClick == EnumOnClick.RemoveToBag);
        }
        
        public void OnSubmit(BaseEventData data)
        {
            if (this.m_OnSubmit == EnumOnSubmit.Nothing) return;
            this.RemoveAttachment(this.m_OnSubmit == EnumOnSubmit.RemoveToBag);
        }

        public void OnSelect(BaseEventData data)
        {
            this.m_ItemUI.OnSelect(this.m_RuntimeSocket?.Attachment);
            
            if (this.m_RuntimeSocket == null) return;
            RuntimeItem.UI_LastSocketSelected = this.m_RuntimeSocket;
        }

        // INTERNAL CALLBACKS: --------------------------------------------------------------------
        
        internal bool OnDropCellUI(BagCellUI dropCellUI)
        {
            if (dropCellUI == null) return false;

            return this.m_OnDrop switch
            {
                EnumOnDrop.Nothing => false,
                EnumOnDrop.AttachWhenEmpty => this.Attach(dropCellUI),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Attach(BagCellUI dropCellUI)
        {
            if (this.m_RuntimeItem == null) return false;
            
            if (this.m_RuntimeSocket.HasAttachment) return false;
            if (this.m_RuntimeItem.Bag == null) return false;

            RuntimeItem attachment = dropCellUI.Cell.Peek();
            bool successAttach = this.m_RuntimeItem.Bag.Equipment.AttachTo(
                this.m_RuntimeItem,
                attachment,
                this.m_RuntimeSocket.SocketID
            );

            if (successAttach) EventSocketAttach?.Invoke(this);
            return successAttach;
        }

        public RuntimeItem RemoveAttachment(bool sendToBag)
        {
            if (this.m_RuntimeItem == null) return null;
            if (!this.m_RuntimeSocket.HasAttachment) return null;
            
            Bag bag = this.m_RuntimeItem.Bag;
            if (bag == null) return null;

            if (sendToBag)
            {
                if (!bag.Content.CanAddType(this.m_RuntimeSocket.Attachment.Item, true))
                {
                    return null;
                }
            }

            RuntimeItem detachment = bag.Equipment.DetachFrom(
                this.m_RuntimeItem, 
                this.m_RuntimeSocket.SocketID
            );

            if (detachment != null)
            {
                if (sendToBag) bag.Content.Add(detachment, true);

                Selectable selectable = this.Get<Selectable>();
                if (selectable != null) selectable.Select();
                
                EventSocketDetach?.Invoke(this);
            }

            return detachment;
        }
    }
}