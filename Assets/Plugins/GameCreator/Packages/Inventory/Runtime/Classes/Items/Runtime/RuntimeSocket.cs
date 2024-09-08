using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class RuntimeSocket
    {
        [SerializeField] private IdString m_SocketID;
        [SerializeField] private IdString m_AttachmentBaseID;
        [SerializeReference] private RuntimeItem m_AttachmentRuntimeItem;

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool HasAttachment => this.m_AttachmentRuntimeItem != null;
        public RuntimeItem Attachment => this.m_AttachmentRuntimeItem;

        public IdString SocketID => this.m_SocketID;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public RuntimeSocket(Socket socket)
        {
            this.m_SocketID = socket.ID;
            this.m_AttachmentBaseID = socket.Base != null
                ? socket.Base.ID
                : IdString.EMPTY;
        }
        
        public RuntimeSocket(RuntimeSocket runtimeSocket)
        {
            this.m_SocketID = runtimeSocket.SocketID;
            this.m_AttachmentBaseID = new IdString(runtimeSocket.m_AttachmentBaseID.String);
            this.m_AttachmentRuntimeItem = runtimeSocket.HasAttachment
                ? new RuntimeItem(runtimeSocket.Attachment, false)
                : null;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        internal bool CanAttach(RuntimeItem attachment)
        {
            return attachment != null && attachment.InheritsFrom(this.m_AttachmentBaseID);
        }
        
        internal bool Attach(RuntimeItem attachment)
        {
            if (!this.CanAttach(attachment)) return false;

            if (this.HasAttachment)
            {
                RuntimeItem detached = this.Detach();
                if (detached == null) return false;
            }

            this.m_AttachmentRuntimeItem = attachment;
            return true;
        }

        internal RuntimeItem Detach()
        {
            if (!this.HasAttachment) return null;
            
            RuntimeItem attachment = this.m_AttachmentRuntimeItem;
            this.m_AttachmentRuntimeItem = null;

            return attachment;
        }
    }
}