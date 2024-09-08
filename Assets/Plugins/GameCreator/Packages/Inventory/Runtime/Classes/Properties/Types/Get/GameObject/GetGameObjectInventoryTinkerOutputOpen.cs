using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Current Tinker Output Bag")]
    [Category("Inventory/Current Tinker Output Bag")]
    
    [Image(typeof(IconCraft), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("The output Bag associated with the current open Tinker UI")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectInventoryTinkerOutputOpen : PropertyTypeGetGameObject
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
            Bag bag = TinkerUI.IsOpen ? TinkerUI.LastBagTinkerOutput : null;
            return bag != null ? bag.gameObject : null;
        }

        public override string String => "Open Tinker Output";
    }
}