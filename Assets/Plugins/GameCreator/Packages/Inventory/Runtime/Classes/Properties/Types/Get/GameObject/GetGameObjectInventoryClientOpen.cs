using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Current Client Bag")]
    [Category("Inventory/Current Client Bag")]
    
    [Image(typeof(IconMerchant), ColorTheme.Type.Blue, typeof(OverlayDot))]
    [Description("The Bag associated with the client of the current open Merchant UI")]

    [Keywords("Merchant")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectInventoryClientOpen : PropertyTypeGetGameObject
    {
        public override GameObject Get(Args args)
        {
            return this.GetValue();
        }

        public override GameObject Get(GameObject gameObject)
        {
            return this.GetValue();
        }

        private GameObject GetValue()
        {
            Bag bag = MerchantUI.IsOpen ? MerchantUI.LastBagClient : null;
            return bag != null ? bag.gameObject : null;
        }

        public override string String => "Open Client";
    }
}