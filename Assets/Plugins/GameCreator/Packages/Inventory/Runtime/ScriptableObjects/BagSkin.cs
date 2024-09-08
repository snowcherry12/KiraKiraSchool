using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [CreateAssetMenu(
        fileName = "Bag Skin",
        menuName = "Game Creator/Developer/Inventory/Bag Skin",
        order = 300
    )]
    
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoBag.png")]
    public class BagSkin : TSkin<GameObject>
    {
        private const string MSG = "A game object prefab with the UI components for a Bag";
        
        private const string ERR_NO_VALUE = "Prefab value cannot be empty";
        private const string ERR_BAG_UI = "Prefab does not contain a 'TBagUI' component";

        public override string Description => MSG;

        public override string HasError
        {
            get
            {
                if (this.Value == null) return ERR_NO_VALUE;
                return !this.Value.GetComponent<TBagUI>() ? ERR_BAG_UI : string.Empty;
            }
        }

        public bool MatchType(Type bagType)
        {
            TBagUI bagUI = this.Value != null ? this.Value.GetComponent<TBagUI>() : null;
            return bagUI != null && bagUI.ExpectedBagType == bagType;
        }
    }
}