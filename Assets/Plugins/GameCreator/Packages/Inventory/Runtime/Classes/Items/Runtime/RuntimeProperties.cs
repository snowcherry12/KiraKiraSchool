using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class RuntimeProperties : TSerializableDictionary<IdString, RuntimeProperty>
    {
        // EVENTS: --------------------------------------------------------------------------------
        
        public event Action EventChangeProperties;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public RuntimeProperties(RuntimeItem parent)
        {
            Dictionary<IdString, Property> properties = Properties.FlattenHierarchy(parent.Item);
            foreach (KeyValuePair<IdString, Property> entry in properties)
            {
                RuntimeProperty runtimeProperty = new RuntimeProperty(parent, entry.Value);
                runtimeProperty.EventChange += this.ExecuteEventChange;
                
                this[entry.Key] = runtimeProperty;
            }
        }
        
        public RuntimeProperties(RuntimeItem parent, RuntimeItem copyFromRuntimeItem)
        {
            foreach (KeyValuePair<IdString, RuntimeProperty> entry in copyFromRuntimeItem.Properties)
            {
                RuntimeProperty runtimeProperty = new RuntimeProperty(parent, entry.Value);
                runtimeProperty.EventChange += this.ExecuteEventChange;
                
                this[entry.Key] = runtimeProperty;
            }
        }

        // INTERNAL METHODS: ----------------------------------------------------------------------
        
        internal void OnAttach(RuntimeItem attachment)
        {
            foreach (KeyValuePair<IdString, RuntimeProperty> entryProperty in attachment.Properties)
            {
                if (!this.ContainsKey(entryProperty.Key)) continue;
                this.ExecuteEventChange();
                return;
            }
        }
        
        internal void OnDetach(RuntimeItem attachment)
        {
            foreach (KeyValuePair<IdString, RuntimeProperty> entryProperty in attachment.Properties)
            {
                if (!this.ContainsKey(entryProperty.Key)) continue;
                this.ExecuteEventChange();
                return;
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void ExecuteEventChange()
        {
            this.EventChangeProperties?.Invoke();
        }
    }
}