using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("0.1")]
    [Category("Constant/Point One")]
    
    [Image(typeof(IconPointOne), ColorTheme.Type.TextNormal)]
    [Description("The unit 0.1 value")]
    
    [Serializable]
    public class GetDecimalConstantPointOne : PropertyTypeGetDecimal
    {
        public override double Get(Args args) => 0.1;
        public override double Get(GameObject gameObject) => 0.1;

        public static PropertyGetDecimal Create => new PropertyGetDecimal(new GetDecimalConstantPointOne());

        public override string String => "0.1";

        public override double EditorValue => 0.1;
    }
}