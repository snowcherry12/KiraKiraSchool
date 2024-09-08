using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Bag Weight Ratio")]
    [Category("Inventory/Weight/Bag Weight Ratio")]
    
    [Image(typeof(IconWeight), ColorTheme.Type.Blue)]
    [Description("A unit ratio between the current weight and maximum weight capacity of a Bag component")]

    [Parameter("Bag", "The targeted Bag component")]
    
    [Keywords("Overload", "Inventory", "Size", "Capacity")]

    [Serializable]
    public class GetDecimalInventoryBagRatioWeight : PropertyTypeGetDecimal
    {
        [SerializeField] protected PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        public override double Get(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return 0;

            float max = bag.Shape.MaxWeight;
            float current = bag.Content.CurrentWeight;
            
            return current / max;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryBagRatioWeight()
        );

        public override string String => $"{this.m_Bag} Weight Ratio";
    }
}