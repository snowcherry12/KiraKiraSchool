using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Stat Sprite")]
    [Category("Stats/Stat Sprite")]
    
    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    [Description("A reference to the Stat Sprite value")]

    [Serializable] [HideLabelsInEditor]
    public class GetSpriteStat : PropertyTypeGetSprite
    {
        [SerializeField] protected PropertyGetStat m_Stat = new PropertyGetStat();

        public override Sprite Get(Args args)
        {
            Stat stat = this.m_Stat.Get(args);
            return stat != null
                ? stat.GetIcon(args)
                : null;
        }

        public override string String => $"{this.m_Stat} Sprite";
    }
}