using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Luminance At")]
    [Category("Perception/Luminance At")]
    
    [Description("The luminance value at a specific game object position")]
    [Image(typeof(IconLuminance), ColorTheme.Type.Yellow)]

    [Serializable]
    public class GetDecimalLuminanceAt : PropertyTypeGetDecimal
    {
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        public override double Get(Args args)
        {
            GameObject target = this.m_Target.Get(args);
            return target != null ? LuminanceManager.Instance.LuminanceAt(target.transform) : 0f;
        }
        
        public override string String => $"{this.m_Target} Luminance";
    }
}