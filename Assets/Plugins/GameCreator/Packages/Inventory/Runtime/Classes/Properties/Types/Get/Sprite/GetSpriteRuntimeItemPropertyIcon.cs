using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Title("Runtime Item Property Sprite")]
    [Category("Inventory/Runtime Item Property Sprite")]
    
    [Image(typeof(IconProperty), ColorTheme.Type.Blue)]
    [Description("Returns the property Sprite of the Runtime Item")]
    
    [Serializable]
    public class GetSpriteRuntimeItemPropertyIcon : PropertyTypeGetSprite
    {
        [SerializeField] private PropertyGetRuntimeItem m_RuntimeItem = new PropertyGetRuntimeItem();
        [SerializeField] private IdString m_PropertyId = IdString.EMPTY;

        public override Sprite Get(Args args) => this.GetSprite(args);

        private Sprite GetSprite(Args args)
        {
            RuntimeItem runtimeItem = this.m_RuntimeItem.Get(args);
            if (runtimeItem == null) return null;

            return runtimeItem.Properties.TryGetValue(this.m_PropertyId, out var property)
                ? property.Icon
                : null;
        }

        public override string String => $"{this.m_RuntimeItem}[{this.m_PropertyId}] Sprite";
    }
}