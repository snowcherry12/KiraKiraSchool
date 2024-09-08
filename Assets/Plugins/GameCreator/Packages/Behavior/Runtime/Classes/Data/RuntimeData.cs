using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Behavior
{
    [Serializable]
    public class RuntimeData : TPolymorphicList<Parameter>
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeReference] private Parameter[] m_Overrides = Array.Empty<Parameter>();
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly Dictionary<PropertyName, Parameter> m_Parameters;
        
        [NonSerialized] private readonly Dictionary<IdString, Status> m_Status;
        [NonSerialized] private readonly Dictionary<IdString, IValue> m_Values;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override int Length => this.m_Overrides.Length;

        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChangeParameter;
        
        public event Action EventChangeStatus;
        public event Action EventChangeEntries;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public RuntimeData()
        {
            this.m_Parameters = new Dictionary<PropertyName, Parameter>();
            
           this.m_Status = new Dictionary<IdString, Status>();
           this.m_Values = new Dictionary<IdString, IValue>();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void OnStartup(Graph graph)
        {
            this.SyncData(graph);

            foreach (Parameter entry in this.m_Overrides)
            {
                this.m_Parameters[entry.Name] = entry.Copy as Parameter;
            }
        }
        
        public void Restart()
        {
            this.m_Status.Clear();
            foreach (KeyValuePair<IdString, IValue> entry in this.m_Values)
            {
                entry.Value.Restart();
            }

            this.EventChangeStatus?.Invoke();
        }

        // PARAMETER METHODS: ---------------------------------------------------------------------

        public object GetParameter(PropertyName id)
        {
            return this.m_Parameters.TryGetValue(id, out Parameter parameter) && parameter != null
                ? parameter.Value
                : null;
        }

        public void SetParameter(PropertyName id, object value)
        {
            if (!this.m_Parameters.TryGetValue(id, out Parameter parameter)) return;
            
            parameter.Value = value;
            this.EventChangeParameter?.Invoke();
        }

        // STATUS METHODS: ------------------------------------------------------------------------

        public Status GetStatus(IdString nodeId)
        {
            return this.m_Status.TryGetValue(nodeId, out Status status) 
                ? status
                : Status.Ready;
        }
        
        public void SetStatus(IdString nodeId, Status status)
        {
            this.m_Status[nodeId] = status;
            this.EventChangeStatus?.Invoke();
        }

        // ENTRY METHODS: -------------------------------------------------------------------------

        public T GetValue<T>(IdString nodeId) where T : class, IValue
        {
            return this.m_Values.TryGetValue(nodeId, out IValue entry) 
                ? entry as T 
                : null;
        }
        
        public void SetValue<T>(IdString nodeId, T entry) where T : IValue
        {
            this.m_Values[nodeId] = entry;
            this.EventChangeEntries?.Invoke();
        }
        
        // SYNC METHODS: --------------------------------------------------------------------------

        internal void SyncData(Graph graph)
        {
            if (graph == null) return;

            List<Parameter> parameters = new List<Parameter>();
            this.SyncData(new HashSet<Graph>(), graph, parameters);
            this.m_Overrides = parameters.ToArray();
        }

        private void SyncData(
            ISet<Graph> previousAssets, 
            Graph asset,
            List<Parameter> parameters)
        {
            if (asset == null) return;
            if (previousAssets.Contains(asset)) return;

            Parameter[] assetList = asset.Data.List;
            Parameter[] instanceList = new Parameter[assetList.Length];

            for (int i = 0; i < assetList.Length; ++i)
            {
                Parameter assetParameter = assetList[i];
                Parameter overrideParameter = null;

                foreach (Parameter candidateParameter in this.m_Overrides)
                {
                    if (candidateParameter.Name != assetParameter.Name) continue;
                    if (candidateParameter.TypeID.Hash != assetParameter.TypeID.Hash) continue;

                    overrideParameter = candidateParameter.Copy as Parameter;
                }
                
                Parameter copy = overrideParameter != null
                    ? overrideParameter.Copy as Parameter
                    : assetList[i].Copy as Parameter;
                
                if (copy == null) continue;
                instanceList[i] = copy;
            }

            previousAssets.Add(asset);
            parameters.AddRange(instanceList);

            foreach (TNode node in asset.Nodes)
            {
                Graph subgraph = node?.Subgraph;
                if (subgraph == null) continue;
                
                SyncData(previousAssets, subgraph, parameters);
            }
        }
    }
}