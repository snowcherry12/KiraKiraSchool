using System;
using System.Globalization;
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
    public class GetStringStatLastChange : PropertyTypeGetString
    {
        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();

        public override string Get(Args args)
        {
            Traits traits = this.m_Traits.Get<Traits>(args);
            return traits != null 
                ? traits.RuntimeStats.LastChange.ToString("0", CultureInfo.InvariantCulture) 
                : string.Empty;
        }

        public static PropertyGetString Create => new PropertyGetString(
            new GetStringStatLastChange()
        );

        public override string String => this.m_Traits.ToString();
    }
}