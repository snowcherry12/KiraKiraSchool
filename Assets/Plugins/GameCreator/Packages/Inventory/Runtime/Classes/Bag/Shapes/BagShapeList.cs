using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class BagShapeList : TBagShapeWithWeight
    {
        [SerializeField] private EnablerInt m_MaxHeight = new EnablerInt(false, 200);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override int MaxWidth => 1;
        public override int MaxHeight => this.m_MaxHeight.IsEnabled
            ? this.m_MaxHeight.Value
            : int.MaxValue;

        public override bool HasInfiniteWidth => false;
        public override bool HasInfiniteHeight => !this.m_MaxHeight.IsEnabled;

        public override bool CanIncreaseWidth => false;
        public override bool CanDecreaseWidth => false;
        
        public override bool CanIncreaseHeight => true;
        public override bool CanDecreaseHeight => false;

        // METHODS: -------------------------------------------------------------------------------

        public override bool IncreaseWidth(int numColumns) => false;
        public override bool DecreaseWidth(int numColumns) => false;

        public override bool IncreaseHeight(int numRows)
        {
            if (!this.CanIncreaseHeight) return false;
            this.m_MaxHeight.Value += numRows;
            
            this.ExecuteEventChange();
            return true;
        }
        
        public override bool DecreaseHeight(int numRows) => false;
    }
}