using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Icon(EditorPaths.PACKAGES + "Behavior/Editor/Gizmos/GizmoGraph.png")]
    
    [Serializable]
    public abstract class Graph : ScriptableObject
    {
        #if UNITY_EDITOR

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeOnLoad()
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(Graph)}");
            foreach (string guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                Graph asset = UnityEditor.AssetDatabase.LoadAssetAtPath<Graph>(path);
                
                if (asset == null) continue;
                
                asset.m_NodeMap = null;
                asset.m_PortMap = null;
            }
        }
        
        #endif
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeReference] protected TNode[] m_Nodes = Array.Empty<TNode>();
        [SerializeField] private Parameters m_Data = new Parameters();
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Dictionary<IdString, TNode> m_NodeMap;
        [NonSerialized] private Dictionary<Connection, TNode> m_PortMap;

        // PROPERTIES: ----------------------------------------------------------------------------

        public TNode[] Nodes => this.m_Nodes;

        public Parameters Data => this.m_Data;

        // PUBIC METHODS: -------------------------------------------------------------------------

        public abstract Status Run(Processor processor);
        public abstract void Abort(Processor processor);
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal TNode GetFromNodeId(IdString nodeId)
        {
            if (this.m_NodeMap == null)
            {
                this.m_NodeMap = new Dictionary<IdString, TNode>();
                foreach (TNode node in this.m_Nodes)
                {
                    this.m_NodeMap[node.Id] = node;
                }
            }
            
            return this.m_NodeMap.TryGetValue(nodeId, out TNode value) 
                ? value
                : null;
        }

        internal TNode GetFromPortId(Connection portId)
        {
            if (this.m_PortMap == null)
            {
                this.m_PortMap = new Dictionary<Connection, TNode>();
                foreach (TNode node in this.m_Nodes)
                {
                    foreach (TInputPort inputPort in node.Ports.Inputs) this.m_PortMap[inputPort.Id] = node;
                    foreach (TOutputPort outputPort in node.Ports.Outputs) this.m_PortMap[outputPort.Id] = node;
                }
            }
            
            return this.m_PortMap.TryGetValue(portId, out TNode value) 
                ? value
                : null;
        }
    }
}