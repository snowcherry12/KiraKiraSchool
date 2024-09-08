using System;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Random from Bag")]
    [Category("Bags/Random from Bag")]
    
    [Image(typeof(IconDice), ColorTheme.Type.Yellow)]
    [Description("A random reference to a Runtime Item of the specified Bag")]

    [Serializable] [HideLabelsInEditor]
    public class GetRuntimeItemRandom : PropertyTypeGetRuntimeItem
    {
        [SerializeField] private PropertyGetGameObject m_Bag = GetGameObjectPlayer.Create();
        
        public override RuntimeItem Get(Args args)
        {
            Bag bag = this.m_Bag.Get<Bag>(args);
            if (bag == null) return null;

            List<Cell> contentList = bag.Content.CellList;
            List<Cell> candidatesList = new List<Cell>();

            foreach (Cell contentCell in contentList)
            {
                if (contentCell.Available) continue;
                candidatesList.Add(contentCell);
            }

            int candidatesListCount = candidatesList.Count;
            
            if (candidatesListCount == 0) return null;

            int randomIndex = UnityEngine.Random.Range(0, candidatesListCount);
            return candidatesList[randomIndex].Peek();
        }

        public static PropertyGetRuntimeItem Create()
        {
            GetRuntimeItemRandom instance = new GetRuntimeItemRandom();
            return new PropertyGetRuntimeItem(instance);
        }

        public override string String => $"Random from {this.m_Bag}";
    }
}