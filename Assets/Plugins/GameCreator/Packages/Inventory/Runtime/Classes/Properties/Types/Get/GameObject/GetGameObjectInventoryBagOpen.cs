using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Current Open Bag")]
    [Category("Inventory/Current Open Bag")]
    
    [Image(typeof(IconBagSolid), ColorTheme.Type.Yellow, typeof(OverlayDot))]
    [Description("The Bag associated with the current open Bag UI")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectInventoryBagOpen : PropertyTypeGetGameObject
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
            Bag bag = TBagUI.IsOpen ? TBagUI.LastBagOpened : null;
            return bag != null ? bag.gameObject : null;
        }

        public override string String => "Open Bag";
    }
}