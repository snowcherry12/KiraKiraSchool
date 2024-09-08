using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Stat Value")]
    [Category("Stats/Stat Value")]

    [Image(typeof(IconStat), ColorTheme.Type.Red)]
    [Description("The Stat value of a game object's Traits component")]

    [Serializable]
    public class GetDecimalStatValue : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetStat m_Stat = new PropertyGetStat();

        public override double Get(Args args)
        {
            Stat stat = this.m_Stat.Get(args);
            if (stat == null) return 0f;
            
            Traits traits = this.m_Traits.Get<Traits>(args);
            if (traits == null) return 0f;

            return traits.RuntimeStats.Get(stat.ID)?.Value ?? 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalStatValue()
        );

        public override string String => $"{this.m_Traits}[{this.m_Stat}]";
    }
}