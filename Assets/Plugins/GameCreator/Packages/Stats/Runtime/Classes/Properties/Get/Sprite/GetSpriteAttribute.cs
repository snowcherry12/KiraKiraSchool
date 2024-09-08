using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Attribute Sprite")]
    [Category("Stats/Attribute Sprite")]
    
    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]
    [Description("A reference to the Attribute Sprite value")]

    [Serializable]
    public class GetSpriteAttribute : PropertyTypeGetSprite
    {
        [SerializeField] protected PropertyGetAttribute m_Attribute = new PropertyGetAttribute();

        public override Sprite Get(Args args)
        {
            Attribute attribute = this.m_Attribute.Get(args);
            return attribute != null
                ? attribute.GetIcon(args)
                : null;
        }

        public override string String => $"{this.m_Attribute} Sprite";
    }
}