using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Class Sprite")]
    [Category("Stats/Class Sprite")]
    
    [Image(typeof(IconClass), ColorTheme.Type.Teal)]
    [Description("A reference to the Class Sprite value")]

    [Serializable] [HideLabelsInEditor]
    public class GetSpriteClass : PropertyTypeGetSprite
    {
        [SerializeField] protected Class m_Class;

        public override Sprite Get(Args args) => this.m_Class != null
            ? this.m_Class.GetSprite(args) 
            : null;

        public override string String => this.m_Class != null
            ? $"{this.m_Class.name} Sprite"
            : "(none)";
    }
}