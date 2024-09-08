using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [CreateAssetMenu(
        fileName = "Currency",
        menuName = "Game Creator/Inventory/Currency"
    )]
    
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoCurrency.png")]
    
    [Serializable]
    public class Currency : ScriptableObject
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private Coins m_Coins = new Coins();

        [SerializeField] private UniqueID m_UniqueID = new UniqueID();

        // PROPERTIES: ----------------------------------------------------------------------------

        public Coins Coins => this.m_Coins;
        
        public IdString ID => this.m_UniqueID.Get;
    }
}