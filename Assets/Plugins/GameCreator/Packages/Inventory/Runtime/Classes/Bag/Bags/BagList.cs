using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("List")]
    [Category("List")]
    
    [Description("Displays items in a vertical list")]
    [Image(typeof(IconBagList), ColorTheme.Type.Green)]
    
    [Serializable]
    public class BagList : TBag
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private BagShapeList m_Shape = new BagShapeList();
        [SerializeField] private BagContentList m_Content = new BagContentList();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IBagShape Shape => this.m_Shape;
        public override IBagContent Content => this.m_Content;
    }
}