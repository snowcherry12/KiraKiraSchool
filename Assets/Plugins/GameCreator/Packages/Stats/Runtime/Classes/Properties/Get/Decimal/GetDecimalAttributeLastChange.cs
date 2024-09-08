using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("Last Attribute Change")]
    [Category("Stats/Last Attribute Change")]

    [Image(typeof(IconAttr), ColorTheme.Type.Yellow)]
    [Description("The difference between the new and the old value of the last Attribute changed")]

    [Serializable]
    public class GetDecimalAttributeLastChange : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetGameObject m_Traits = GetGameObjectPlayer.Create();

        public override double Get(Args args)
        {
            Traits traits = this.m_Traits.Get<Traits>(args);
            return traits != null ? traits.RuntimeAttributes.LastChange : 0f;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalAttributeLastChange()
        );

        public override string String => this.m_Traits.ToString();
    }
}