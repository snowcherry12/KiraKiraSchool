using System;
using System.Globalization;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Stat Base")]
    [Category("Stats/Stat Base")]

    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    [Description("Returns the base value of a Stat")]
    
    [Serializable]
    public class GetStringStatBase : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();
        [SerializeField] protected PropertyGetStat m_Stat = new PropertyGetStat();

        public override string Get(Args args)
        {
            Stat stat = this.m_Stat.Get(args);
            if (stat == null) return string.Empty;
            
            Traits traits = this.m_Traits.Get<Traits>(args);
            if (traits == null) return string.Empty;

            return traits.RuntimeStats
                .Get(stat.ID)?
                .Base.ToString("0", CultureInfo.InvariantCulture) ?? string.Empty;
        }

        public override string String => $"{this.m_Traits}[{this.m_Stat}].Base";
    }
}