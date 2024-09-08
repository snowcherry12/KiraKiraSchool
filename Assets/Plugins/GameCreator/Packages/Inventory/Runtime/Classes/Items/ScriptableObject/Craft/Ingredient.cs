using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class Ingredient : TPolymorphicItem<Ingredient>
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private Item m_Item;
        [SerializeField] private int m_Amount = 1;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Item Item => this.m_Item;
        public int Amount => this.m_Amount;
    }
}