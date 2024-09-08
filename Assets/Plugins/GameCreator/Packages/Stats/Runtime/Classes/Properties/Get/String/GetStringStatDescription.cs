using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Stat Description")]
    [Category("Stats/Stat Description")]

    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    [Description("Returns the description text of a Stat")]
    
    [Serializable]
    public class GetStringStatDescription : PropertyTypeGetString
    {
        [SerializeField] protected PropertyGetStat m_Stat = new PropertyGetStat();

        public override string Get(Args args)
        {
            Stat stat = this.m_Stat.Get(args);
            
            return stat != null
                ? stat.GetDescription(args)
                : string.Empty;
        }

        public override string String => this.m_Stat.ToString();
    }
}