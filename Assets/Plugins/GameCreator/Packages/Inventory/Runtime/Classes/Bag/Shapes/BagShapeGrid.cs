using System;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class BagShapeGrid : TBagShapeWithWeight
    {
        [SerializeField] private int m_Width = 8;
        [SerializeField] private int m_Height = 6;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override int MaxWidth => this.m_Width;
        public override int MaxHeight => this.m_Height;

        public override bool HasInfiniteWidth => false;
        public override bool HasInfiniteHeight => false;

        public override bool CanIncreaseWidth => true;
        public override bool CanDecreaseWidth => false;
        
        public override bool CanIncreaseHeight => true;
        public override bool CanDecreaseHeight => false;

        // METHODS: -------------------------------------------------------------------------------
        
        public override bool IncreaseWidth(int numColumns)
        {
            if (!this.CanIncreaseWidth) return false;
            this.m_Width += numColumns;
            
            this.ExecuteEventChange();
            return true;
        }

        public override bool DecreaseWidth(int numColumns) => false;

        public override bool IncreaseHeight(int numRows)
        {
            if (!this.CanIncreaseHeight) return false;
            this.m_Height += numRows;
            
            this.ExecuteEventChange();
            return true;
        }
        
        public override bool DecreaseHeight(int numRows) => false;
    }
}