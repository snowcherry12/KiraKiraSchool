using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Is Overloaded")]
    [Description("Returns true if the Bag's maximum weight is surpassed")]

    [Category("Inventory/Is Overloaded")]
    [Parameter("Bag", "The Bag component")]

    [Keywords("Inventory", "Weight", "Amount")]
    [Image(typeof(IconWeight), ColorTheme.Type.Red, typeof(OverlayArrowUp))]
    
    [Serializable]
    public class ConditionInventoryBagOverloaded : Condition
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] protected PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is {this.m_Bag} Overloaded";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return false;

            int max = bag.Shape.MaxWeight;
            int current = bag.Content.CurrentWeight;

            return current > max;
        }
    }
}
