using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [CreateAssetMenu(
        fileName = "Merchant Skin",
        menuName = "Game Creator/Developer/Inventory/Merchant Skin",
        order = 300
    )]
    
    [Icon(RuntimePaths.PACKAGES + "Inventory/Editor/Gizmos/GizmoMerchant.png")]
    public class MerchantSkin : TSkin<GameObject>
    {
        private const string MSG = "A game object prefab with the UI components for a Merchant";
        
        private const string ERR_NO_VALUE = "Prefab value cannot be empty";
        private const string ERR_BAG_UI = "Prefab does not contain a 'MerchantUI' component";

        public override string Description => MSG;

        public override string HasError
        {
            get
            {
                if (this.Value == null) return ERR_NO_VALUE;
                return !this.Value.Get<MerchantUI>() ? ERR_BAG_UI : string.Empty;
            }
        }
    }
}