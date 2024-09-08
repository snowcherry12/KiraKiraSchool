using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Din Intensity At")]
    [Category("Perception/Din Intensity At")]
    
    [Description("The ambient noise intensity value for a specific Perception component")]
    [Image(typeof(IconStorm), ColorTheme.Type.Red, typeof(OverlayArrowRight))]

    [Serializable]
    public class GetDecimalDinIntensityAt : PropertyTypeGetDecimal
    {
        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        public override double Get(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            if (perception == null) return 0;
            
            return HearManager.Instance.DinFor(perception);
        }
        
        public override string String => $"{this.m_Perception} Din";
    }
}