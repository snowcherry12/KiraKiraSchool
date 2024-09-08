using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Grid")]
    [Category("Grid")]
    
    [Description("Items are placed in a grid. Each item can occupy more than one cell")]
    [Image(typeof(IconBagGrid), ColorTheme.Type.Green)]
    
    [Serializable]
    public class BagGrid : TBag
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private BagShapeGrid m_Shape = new BagShapeGrid();
        [SerializeField] private BagContentGrid m_Content = new BagContentGrid();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IBagShape Shape => this.m_Shape;
        public override IBagContent Content => this.m_Content;
    }
}