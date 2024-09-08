using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Stat")]
    [Category("Stat")]
    
    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    [Description("A direct reference to the Stat value")]

    [Serializable] [HideLabelsInEditor]
    public class GetStatInstance : PropertyTypeGetStat
    {
        [SerializeField] protected Stat m_Stat;

        public override Stat Get(Args args) => this.m_Stat;
        public override Stat Get(GameObject gameObject) => this.m_Stat;
        
        public override string String => this.m_Stat != null
            ? $"{this.m_Stat.name}"
            : "(none)";
    }
}