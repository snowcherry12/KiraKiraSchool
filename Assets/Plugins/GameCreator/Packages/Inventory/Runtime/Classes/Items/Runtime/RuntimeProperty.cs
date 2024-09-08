using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class RuntimeProperty
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private IdString m_ID;

        [SerializeField] private float m_Number;
        [SerializeField] private string m_Text;

        // PROPERTIES: ----------------------------------------------------------------------------

        public IdString ID => this.m_ID;

        public float Number
        {
            get => this.m_Number;
            set
            {
                this.m_Number = value;
                EventChange?.Invoke();
            }
        }

        public string Text
        {
            get => this.m_Text;
            set
            {
                this.m_Text = value;
                EventChange?.Invoke();
            }
        }

        [field: NonSerialized] public Sprite Icon { get; }
        [field: NonSerialized] public Color Color { get; }
        
        // EVENTS: --------------------------------------------------------------------------------

        internal event Action EventChange;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public RuntimeProperty(RuntimeItem runtimeItem, Property property)
        {
            this.m_ID = property.ID;

            this.m_Number = property.Number;
            this.m_Text = property.Text(runtimeItem.Bag != null ? runtimeItem.Bag.Args : null);
            
            this.Icon = property.Icon;
            this.Color = property.Tint;
        }
        
        public RuntimeProperty(RuntimeItem runtimeItem, RuntimeProperty runtimeProperty)
        {
            this.m_ID = runtimeProperty.ID;
            this.m_Number = runtimeProperty.Number;
            this.m_Text = runtimeProperty.Text;
            
            this.Icon = runtimeProperty.Icon;
            this.Color = runtimeProperty.Color;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool IsVisible(RuntimeItem runtimeItem)
        {
            Property property = runtimeItem.Item.Properties.Get(this.m_ID, runtimeItem.Item);
            return property.Visible switch
            {
                Property.Visibility.AlwaysVisible => true,
                Property.Visibility.HideIfZero => this.GetTotalNumber(runtimeItem) > 0,
                Property.Visibility.Hidden => false,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public float GetTotalNumber(RuntimeItem runtimeItem)
        {
            float value = this.Number;

            foreach (KeyValuePair<IdString, RuntimeSocket> entrySocket in runtimeItem.Sockets)
            {
                if (!entrySocket.Value.HasAttachment) continue;
                
                RuntimeItem attachment = entrySocket.Value.Attachment;
                if (attachment.Properties.TryGetValue(this.m_ID, out RuntimeProperty property))
                {
                    value += property.GetTotalNumber(attachment);
                }
            }
                
            return value;
        }
        
        public bool Equivalent(RuntimeProperty other)
        {
            return Mathf.Approximately(this.Number, other.Number) && this.ID.Hash == other.ID.Hash;
        }
        
        public bool Equivalent(Property other)
        {
            return Mathf.Approximately(this.Number, other.Number) && this.ID.Hash == other.ID.Hash;
        }
    }
}