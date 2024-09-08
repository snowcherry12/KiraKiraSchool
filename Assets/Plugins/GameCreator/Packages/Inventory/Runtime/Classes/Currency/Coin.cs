using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Coin : TPolymorphicItem<Coin>
    {
        [SerializeField] private int m_Value;
        
        [SerializeField] private PropertyGetString m_Name;
        [SerializeField] private PropertyGetSprite m_Icon;
        [SerializeField] private PropertyGetColor m_Tint;

        // PROPERTIES: ----------------------------------------------------------------------------

        public int Multiplier => this.m_Value;
        
        public string Name => this.m_Name.Get(Args.EMPTY);
        public Sprite Icon => this.m_Icon.Get(Args.EMPTY);
        public Color Tint => this.m_Tint.Get(Args.EMPTY);

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Coin() : this(string.Empty, 1)
        { }

        public Coin(string name, int value)
        {
            this.m_Value = value;
            
            this.m_Name = new PropertyGetString(name);
            this.m_Icon = new PropertyGetSprite();
            this.m_Tint = GetColorColorsWhite.Create;
        }
        
        // STRING METHODS: ------------------------------------------------------------------------

        public override string ToString() => $"{this.m_Name}: {this.m_Value}";
    }
}