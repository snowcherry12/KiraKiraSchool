using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Class Color")]
    [Category("Stats/Class Color")]
    
    [Image(typeof(IconClass), ColorTheme.Type.Teal)]
    [Description("Returns the Color value of a Class")]

    [Serializable] [HideLabelsInEditor]
    public class GetColorClass : PropertyTypeGetColor
    {
        [SerializeField] private Class m_Class;

        public override Color Get(Args args)
        {
            return this.m_Class != null ? this.m_Class.GetColor(args) : Color.black;
        }

        public override string String => this.m_Class != null
            ? $"{this.m_Class.name} Color"
            : "(none)";
    }
}