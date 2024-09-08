using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class RuntimeAttributes
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        private Traits m_Traits;
        private Dictionary<int, RuntimeAttributeData> m_Attributes;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public int Count => this.m_Attributes.Count;
        
        /// <summary>
        /// Returns the difference between the the previous and new value from the last
        /// modified Attribute.
        /// </summary>
        public double LastChange { get; private set; }
        
        public List<int> AttributesKeys => new List<int>(this.m_Attributes.Keys);
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action<IdString> EventChange;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        internal RuntimeAttributes(Traits traits)
        {
            this.m_Traits = traits;
            this.m_Attributes = new Dictionary<int, RuntimeAttributeData>();
        }
        
        internal RuntimeAttributes(Traits traits, OverrideAttributes overrideAttributes) : this(traits)
        {
            for (int i = 0; i < this.m_Traits.Class.AttributesLength; ++i)
            {
                AttributeItem attribute = this.m_Traits.Class.GetAttribute(i);
                if (attribute == null || attribute.Attribute == null)
                {
                    const string error = "No Attribute reference found";
                    throw new NullReferenceException(error);
                }

                IdString attributeID = attribute.Attribute.ID;
                if (this.m_Attributes.ContainsKey(attributeID.Hash))
                {
                    string error = $"Duplicate Attribute '{attributeID.String}' has already been defined";
                    throw new Exception(error);
                }
                
                RuntimeAttributeData data = new RuntimeAttributeData(this.m_Traits, attribute);
                if (!attribute.IsHidden && 
                    overrideAttributes.TryGetValue(attributeID, out OverrideAttributeData overrideData))
                {
                    if (overrideData.ChangeStartPercent) 
                    {
                        double value = MathUtils.Lerp(
                            data.MinValue,
                            data.MaxValue,
                            overrideData.StartPercent
                        );
                        
                        data.SetValueWithoutNotify(value);
                    }
                }
                
                data.EventChange += this.ExecuteEventChange;
                this.m_Attributes[attributeID.Hash] = data;
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void ExecuteEventChange(IdString attributeID, double change)
        {
            this.LastChange = change;
            this.EventChange?.Invoke(attributeID);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        /// <summary>
        /// Returns the RuntimeAttributeData instance of a requested attribute. Throws an
        /// exception if the requested attribute cannot be found.
        /// </summary>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        public RuntimeAttributeData Get(IdString attributeID)
        {
            if (this.m_Attributes == null) return null;
            if (this.m_Attributes.TryGetValue(attributeID.Hash, out RuntimeAttributeData attribute))
            {
                return attribute;
            }
        
            string objectName = this.m_Traits.gameObject.name;
            throw new Exception($"Cannot find Attribute '{attributeID.String}' in {objectName}");
        }

        /// <summary>
        /// Returns the RuntimeAttributeData instance of a requested attribute. Throws an
        /// exception if the requested attribute cannot be found.
        /// </summary>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        public RuntimeAttributeData Get(string attributeID)
        {
            return this.Get(new IdString(attributeID));
        }
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        /// <summary>
        /// Returns the RuntimeAttributeData instance of a requested attribute.
        /// </summary>
        /// <param name="attrHash"></param>
        /// <returns></returns>
        internal RuntimeAttributeData Get(int attrHash)
        {
            return this.m_Attributes.TryGetValue(attrHash, out RuntimeAttributeData stat) 
                ? stat 
                : null;
        }
    }
}