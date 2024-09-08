using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class RuntimeSockets : TSerializableDictionary<IdString, RuntimeSocket>
    {
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode()
        {
            EventAttachRuntimeItem = null;
            EventDetachRuntimeItem = null;
        }
        
        #endif
        
        // EVENTS: --------------------------------------------------------------------------------
     
        public static event Action<RuntimeItem, RuntimeItem> EventAttachRuntimeItem;
        public static event Action<RuntimeItem, RuntimeItem> EventDetachRuntimeItem;
        
        public event Action EventChangeSocket;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public RuntimeSockets(RuntimeItem runtimeItem)
        {
            Dictionary<IdString, Socket> sockets = Sockets.FlattenHierarchy(runtimeItem.Item);
            foreach (KeyValuePair<IdString, Socket> entry in sockets)
            {
                this[entry.Key] = new RuntimeSocket(entry.Value);
            }
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Attach(RuntimeItem runtimeItem, RuntimeItem attachment)
        {
            IdString socketID = IdString.EMPTY;
            foreach (KeyValuePair<IdString, RuntimeSocket> entry in this)
            {
                if (!entry.Value.CanAttach(attachment)) continue;
                
                socketID = entry.Key;
                if (!entry.Value.HasAttachment) break;
            }

            return this.AttachToSocket(runtimeItem, socketID, attachment);
        }

        public bool Detach(RuntimeItem runtimeItem, RuntimeItem attachment)
        {
            if (attachment == null) return false;
            
            foreach (KeyValuePair<IdString, RuntimeSocket> entry in this)
            {
                if (entry.Value.Attachment.RuntimeID.Hash != attachment.RuntimeID.Hash) continue;
                return this.DetachFromSocket(runtimeItem, entry.Key) != null;
            }

            return false;
        }

        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal void CopyFrom(RuntimeItem runtimeItem)
        {
            foreach (KeyValuePair<IdString, RuntimeSocket> entry in runtimeItem.Sockets)
            {
                this[entry.Key] = new RuntimeSocket(entry.Value);
            }
        }
        
        internal bool AttachToSocket(RuntimeItem runtimeItem, IdString socketID, RuntimeItem attachment)
        {
            if (!this.TryGetValue(socketID, out RuntimeSocket runtimeSocket)) return false;
            
            bool success = runtimeSocket.Attach(attachment);
            if (!success) return false;
                
            RuntimeItem.Socket_LastParentAttached = runtimeItem;
            RuntimeItem.Socket_LastAttachmentAttached = attachment;
                    
            runtimeItem.Properties.OnAttach(attachment);
            this.EventChangeSocket?.Invoke();
            EventAttachRuntimeItem?.Invoke(runtimeItem, attachment);
            
            return true;
        }
        
        internal RuntimeItem DetachFromSocket(RuntimeItem parent, IdString socketID)
        {
            if (!this.TryGetValue(socketID, out RuntimeSocket runtimeSocket)) return null;
            
            RuntimeItem attachment = runtimeSocket.Detach();
            if (attachment == null) return null;
                
            RuntimeItem.Socket_LastParentDetached = parent;
            RuntimeItem.Socket_LastAttachmentDetached = attachment;
                    
            parent.Properties.OnDetach(attachment);
            this.EventChangeSocket?.Invoke();
            EventDetachRuntimeItem?.Invoke(parent, attachment);
            
            return attachment;
        }
    }
}