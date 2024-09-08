using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [AddComponentMenu("Game Creator/Inventory/Prop")]
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoProp.png")]
    
    [Serializable]
    public class Prop : MonoBehaviour, ISerializationCallbackReceiver
    {
        [Serializable]
        public class PropSockets : TSerializableDictionary<IdString, Transform>
        { }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Item m_Item;

        [SerializeField] private PropSockets m_Sockets = new PropSockets();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private RuntimeItem m_RuntimeItem;

        // INITIALIZERS: --------------------------------------------------------------------------

        public static Prop Create(GameObject gameObject, RuntimeItem runtimeItem)
        {
            Prop prop = gameObject.Add<Prop>();
            prop.Setup(runtimeItem);

            return prop;
        }

        private void OnDestroy()
        {
            if (this.m_RuntimeItem != null)
            {
                this.m_RuntimeItem.Sockets.EventChangeSocket -= this.RefreshSockets;
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Setup(RuntimeItem runtimeItem)
        {
            this.m_RuntimeItem = runtimeItem;
            this.m_Item = runtimeItem.Item;
            
            this.SyncSockets();
            
            runtimeItem.Sockets.EventChangeSocket -= this.RefreshSockets;
            runtimeItem.Sockets.EventChangeSocket += this.RefreshSockets;
            
            this.RefreshSockets();
        }
        
        public void RefreshSockets()
        {
            foreach (KeyValuePair<IdString, RuntimeSocket> socketEntry in this.m_RuntimeItem.Sockets)
            {
                IdString socketID = socketEntry.Value.SocketID;
                
                if (this.m_Sockets.TryGetValue(socketID, out Transform socket))
                {
                    if (socket == null) continue;
                    
                    if (socketEntry.Value.HasAttachment)
                    {
                        GameObject prefab = socketEntry.Value.Attachment.Item.Sockets.PrefabSocket;
                        if (prefab != null)
                        {
                            if (socketEntry.Value.Attachment.PropInstance != null)
                            {
                                Destroy(socketEntry.Value.Attachment.PropInstance);
                            }
                            
                            socketEntry.Value.Attachment.PropInstance = Instantiate(
                                prefab, socket
                            );
                        }
                    }
                    else
                    {
                        if (socket != null)
                        {
                            for (int i = socket.childCount - 1; i >= 0; --i)
                            {
                                Destroy(socket.GetChild(i).gameObject);
                            }
                        }
                    }
                }
            }
        }
        
        // SERIALIZATION: -------------------------------------------------------------------------

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (AssemblyUtils.IsReloading) return;
            this.SyncSockets();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        { }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void SyncSockets()
        {
            if (this.m_Item == null)
            {
                this.m_Sockets = new PropSockets();
                return;
            }

            Dictionary<IdString, Socket> sockets = Sockets.FlattenHierarchy(this.m_Item);
            HashSet<IdString> removeCandidates = new HashSet<IdString>(this.m_Sockets.Keys);
            
            foreach (KeyValuePair<IdString,Socket> entry in sockets)
            {
                removeCandidates.Remove(entry.Key);
                
                if (this.m_Sockets.ContainsKey(entry.Key)) continue;
                this.m_Sockets.Add(entry.Key, null);
            }

            foreach (IdString removeCandidate in removeCandidates)
            {
                this.m_Sockets.Remove(removeCandidate);
            }
        }
    }
}