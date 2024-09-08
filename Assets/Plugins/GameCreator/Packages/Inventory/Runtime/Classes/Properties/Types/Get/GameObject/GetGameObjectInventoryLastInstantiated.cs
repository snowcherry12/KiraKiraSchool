using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Last Item Instantiated")]
    [Category("Inventory/Last Item Instantiated")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Green)]
    [Description("The game object instance from the last Item instantiated in the scene")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectInventoryLastInstantiated : PropertyTypeGetGameObject
    {
        public override GameObject Get(Args args) => Item.LastItemInstanceInstantiated;
        
        public override GameObject Get(GameObject gameObject) => Item.LastItemInstanceInstantiated;

        public override string String => "Item[Last Instantiated]";
    }
}