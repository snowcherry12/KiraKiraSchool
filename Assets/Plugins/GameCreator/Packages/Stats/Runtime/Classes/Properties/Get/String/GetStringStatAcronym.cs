using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Stat Acronym")]
    [Category("Stats/Stat Acronym")]

    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    [Description("Returns the acronym of a Stat")]
    
    [Serializable]
    public class GetStringStatAcronym : PropertyTypeGetString
    {
        [SerializeField] protected PropertyGetStat m_Stat = new PropertyGetStat();

        public override string Get(Args args)
        {
            Stat stat = this.m_Stat.Get(args);
            return stat != null
                ? stat.GetAcronym(args)
                : string.Empty;
        }

        public override string String => this.m_Stat.ToString();
    }
}