using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Property : TPolymorphicItem<Property>, IItemListEntry
    {
        public enum Visibility
        {
            AlwaysVisible,
            HideIfZero,
            Hidden
        }
        
        [Flags]
        public enum HideElement
        {
            Number = 1 << 0,
            Text   = 1 << 1,
            Icon   = 1 << 2
        }
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private IdString m_PropertyID;
        [SerializeField] private Visibility m_Visible = Visibility.HideIfZero;

        [SerializeField] private HideElement m_Hide = 0;
        
        [SerializeField] private Sprite m_Icon;
        [SerializeField] private Color m_Color = Color.white;
        
        [SerializeField] private float m_Number;
        [SerializeField] private PropertyGetString m_Text = new PropertyGetString();

        // PROPERTIES: ----------------------------------------------------------------------------

        public Property Clone => new Property
        {
            m_PropertyID = this.m_PropertyID,
            m_Icon = this.m_Icon,
            m_Color = this.m_Color,
            m_Number = this.m_Number,
            m_Text = this.m_Text
        };

        public IdString ID => this.m_PropertyID;
        public Visibility Visible => this.m_Visible;
        public HideElement Hide => this.m_Hide;
        
        public Sprite Icon => this.m_Icon;
        public Color Tint => this.m_Color;
        
        public float Number
        {
            get => this.m_Number;
            internal set => this.m_Number = value;
        }

        public string Text(Args args) => this.m_Text.Get(args);
    }
}