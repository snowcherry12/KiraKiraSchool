using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Bag Max Weight")]
    [Category("Inventory/Weight/Bag Max Weight")]
    
    [Image(typeof(IconWeight), ColorTheme.Type.Yellow)]
    [Description("The the maximum weight of a Bag component")]

    [Parameter("Bag", "The targeted Bag component")]
    
    [Keywords("Overload", "Inventory", "Size", "Capacity")]

    [Serializable]
    public class GetDecimalInventoryBagMaxWeight : PropertyTypeGetDecimal
    {
        [SerializeField] protected PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();

        public override double Get(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return 0f;

            return bag.Shape.MaxWeight;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalInventoryBagMaxWeight()
        );

        public override string String => $"{this.m_Bag} Max Weight";
    }
}