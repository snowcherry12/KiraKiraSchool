using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Stat Name")]
    [Category("Stats/Stat Name")]

    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    [Description("Returns the name of a Stat")]
    
    [Serializable]
    public class GetStringStatName : PropertyTypeGetString
    {
        [SerializeField] protected PropertyGetStat m_Stat = new PropertyGetStat();

        public override string Get(Args args)
        {
            Stat stat = this.m_Stat.Get(args);
            return stat != null
                ? stat.GetName(args)
                : string.Empty;
        }

        public override string String => this.m_Stat.ToString();
    }
}