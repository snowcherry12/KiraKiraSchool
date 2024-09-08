using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Enough Space")]
    [Description("Returns true if the item can be added to the Bag component")]

    [Category("Inventory/Bags/Enough Space")]
    
    [Parameter("Bag", "The Bag to check")]
    [Parameter("Min Space", "The minimum amount of free spaces")]

    [Keywords("Inventory", "Has", "Free", "Available", "Full", "Empty")]
    
    [Image(typeof(IconBagOutline), ColorTheme.Type.Green, typeof(OverlayTick))]
    [Serializable]
    public class ConditionInventoryAvailableSpace : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetInteger m_MinSpace = new PropertyGetInteger(1);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"has {this.m_Bag} {this.m_MinSpace} available or more";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            int minSpace = (int) this.m_MinSpace.Get(args);
            Bag bag = this.m_Bag.Get<Bag>(args);
            
            return bag != null && bag.Content.CountFreeSpaces() >= minSpace;
        }
    }
}
