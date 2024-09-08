using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Bag Current Weight")]
    [Category("Inventory/Weight/Bag Current Weight")]
    
    [Image(typeof(IconWeight), ColorTheme.Type.Yellow, typeof(OverlayArrowDown))]
    [Description("The the current weight of a Bag component")]

    [Parameter("Bag", "The targeted Bag component")]
    
    [Keywords("Overload", "Inventory", "Size", "Capacity")]

    [Serializable]
    public class GetDecimalInventoryBagCurrentWeight : PropertyTypeGetDecimal
    {
        [SerializeField] protected PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        public override double Get(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return 0f;

            return bag.Content.CurrentWeight;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryBagCurrentWeight()
        );

        public override string String => $"{this.m_Bag} Current Weight";
    }
}