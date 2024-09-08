using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Properties : TItemList<Property>
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertiesOverrides m_Overrides = new PropertiesOverrides();

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public Properties() : base()
        { }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool IsInherited(IdString propertyID)
        {
            foreach (Property property in this.m_List)
            {
                if (property.ID.Hash == propertyID.Hash) return false;
            }

            return true;
        }

        public Property Get(IdString propertyID, Item item)
        {
            if (item == null) return null;
            foreach (Property property in item.Properties.m_List)
            {
                if (property.ID.Hash != propertyID.Hash) continue;
                return property;
            }
            
            return item.Parent != null 
                ? item.Parent.Properties.Get(propertyID, item.Parent) 
                : null;
        }

        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static Dictionary<IdString, Property> FlattenHierarchy(Item item)
        {
            Dictionary<IdString, Property> map = new Dictionary<IdString, Property>();
            if (item == null) return map;
            
            foreach (Property listItem in item.Properties.m_List) map[listItem.ID] = listItem.Clone;
        
            if (item.Parent != null && item.Properties.InheritFromParent)
            {
                Dictionary<IdString, Property> parentList = FlattenHierarchy(item.Parent);
                foreach (KeyValuePair<IdString, Property> entry in parentList)
                {
                    if (!map.ContainsKey(entry.Key))
                    {
                        map[entry.Key] = entry.Value;

                        PropertiesOverrides overrides = item.Properties.m_Overrides; 
                        if (overrides.TryGetValue(entry.Key, out PropertyOverride replacement))
                        {
                            if (replacement.Override) map[entry.Key].Number = replacement.Number;
                        }
                    }
                }
            }
        
            return map;
        }
        
        // SERIALIZATION CALLBACK: ----------------------------------------------------------------
        
        internal void OnBeforeSerialize(Item item)
        {
            if (item.Parent == null || !item.Properties.InheritFromParent)
            {
                this.m_Overrides = new PropertiesOverrides();
                return;
            }

            PropertiesOverrides overrides = new PropertiesOverrides();
            
            Dictionary<IdString, Property> parentList = FlattenHierarchy(item.Parent);
            foreach (KeyValuePair<IdString, Property> entry in parentList)
            {
                overrides[entry.Key] = this.m_Overrides.ContainsKey(entry.Key)
                    ? this.m_Overrides[entry.Key]
                    : new PropertyOverride();
            }
            
            this.m_Overrides = overrides;
        }
    }
}