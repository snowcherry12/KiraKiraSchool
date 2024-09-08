using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Last Stat Change")]
    [Category("Stats/Last Stat Change")]

    [Image(typeof(IconStat), ColorTheme.Type.Yellow)]
    [Description("The difference between the new and the old value of the last Stat changed")]

    [Serializable]
    public class GetDecimalStatLastChange : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();

        public override double Get(Args args)
        {
            Traits traits = this.m_Traits.Get<Traits>(args);
            return traits != null ? traits.RuntimeStats.LastChange : 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalStatLastChange()
        );

        public override string String => this.m_Traits.ToString();
    }
}