using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Stat Color")]
    [Category("Stats/Stat Color")]
    
    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    [Description("Returns the Color value of a Stat")]

    [Serializable] [HideLabelsInEditor]
    public class GetColorStat : PropertyTypeGetColor
    {
        [SerializeField] private PropertyGetStat m_Stat = new PropertyGetStat();

        public override Color Get(Args args)
        {
            Stat stat = this.m_Stat.Get(args);
            return stat != null
                ? stat.Color
                : Color.black;
        }

        public override string String => this.m_Stat.ToString();
    }
}