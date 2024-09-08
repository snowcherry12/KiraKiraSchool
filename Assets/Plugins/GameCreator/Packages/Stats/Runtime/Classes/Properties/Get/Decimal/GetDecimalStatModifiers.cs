using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Stat Modifiers")]
    [Category("Stats/Stat Modifiers")]

    [Image(typeof(IconStat), ColorTheme.Type.Red, typeof(OverlayDot))]
    [Description("The amount all Stat Modifiers contribute to a game object's Stat value")]

    [Serializable]
    public class GetDecimalStatModifiers : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetStat m_Stat = new PropertyGetStat();

        public override double Get(Args args)
        {
            Stat stat = this.m_Stat.Get(args);
            if (stat == null) return 0f;
            
            Traits traits = this.m_Traits.Get<Traits>(args);
            if (traits == null) return 0f;

            return traits.RuntimeStats.Get(stat.ID)?.ModifiersValue ?? 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalStatModifiers()
        );

        public override string String => $"{this.m_Traits}[{this.m_Stat}]";
    }
}