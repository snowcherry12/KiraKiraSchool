using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Current Merchant Bag")]
    [Category("Inventory/Current Merchant Bag")]
    
    [Image(typeof(IconMerchant), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("The Bag associated with the current open Merchant UI")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectInventoryMerchantOpen : PropertyTypeGetGameObject
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
            Bag bag = MerchantUI.IsOpen ? MerchantUI.LastBagMerchant : null;
            return bag != null ? bag.gameObject : null;
        }

        public override string String => "Open Merchant";
    }
}