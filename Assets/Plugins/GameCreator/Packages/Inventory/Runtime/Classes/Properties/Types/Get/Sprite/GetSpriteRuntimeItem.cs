using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Runtime Item Sprite")]
    [Category("Inventory/Runtime Item Sprite")]
    
    [Image(typeof(IconItem), ColorTheme.Type.Blue)]
    [Description("Returns the Sprite of the Runtime Item")]
    
    [Serializable]
    public class GetSpriteRuntimeItem : PropertyTypeGetSprite
    {
        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();

        public override Sprite Get(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            return runtimeItem?.Item.Info.Sprite(args);
        }

        public override string String => $"{this.m_RuntimeItem} Sprite";
    }
}