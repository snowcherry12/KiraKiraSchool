using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Stat Base")]
    [Category("Stats/Stat Base")]

    [Image(typeof(IconStat), ColorTheme.Type.Red, typeof(OverlayBar))]
    [Description("The base Stat value of a game object's Traits component")]

    [Serializable]
    public class GetDecimalStatBase : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetStat m_Stat = new PropertyGetStat();

        public override double Get(Args args)
        {
            Stat stat = this.m_Stat.Get(args);
            if (stat == null) return 0f;
            
            Traits traits = this.m_Traits.Get<Traits>(args);
            if (traits == null) return 0f;

            return traits.RuntimeStats.Get(stat.ID)?.Base ?? 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalStatBase()
        );

        public override string String => $"{this.m_Traits}[{this.m_Stat}].Base";
    }
}