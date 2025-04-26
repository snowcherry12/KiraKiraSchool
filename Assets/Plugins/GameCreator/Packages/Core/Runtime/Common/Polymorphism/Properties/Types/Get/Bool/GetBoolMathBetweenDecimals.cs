using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Between Decimals")]
    [Category("Math/Between Decimals")]
    
    [Image(typeof(IconPercent), ColorTheme.Type.Red, typeof(OverlayBar))]
    [Description("Returns True if the value is between Min and Max")]
    
    [Keywords("Compare", "Range")]
    [Serializable]
    public class GetBoolMathBetweenDecimals : PropertyTypeGetBool
    {
        [SerializeField] private PropertyGetDecimal m_Value = new PropertyGetDecimal();
        [SerializeField] private PropertyGetDecimal m_Min = new PropertyGetDecimal();
        [SerializeField] private PropertyGetDecimal m_Max = new PropertyGetDecimal();
        
        public override bool Get(Args args)
        {
            double value = this.m_Value.Get(args);
            double min = this.m_Min.Get(args);
            double max = this.m_Max.Get(args);
            
            return value >= min && value <= max;
        }

        public static PropertyGetBool Create => new PropertyGetBool(
            new GetBoolMathBetweenDecimals()
        );

        public override string String => $"{this.m_Value} between [{this.m_Min}, {this.m_Max}]";
    }
}