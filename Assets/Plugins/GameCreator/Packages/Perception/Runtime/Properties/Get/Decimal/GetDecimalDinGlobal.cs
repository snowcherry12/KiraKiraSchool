using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Perception
{
    [Title("Din Global")]
    [Category("Perception/Din Global")]
    
    [Description("The global ambient noise intensity")]
    [Image(typeof(IconStorm), ColorTheme.Type.Red)]

    [Serializable]
    public class GetDecimalDinGlobal : PropertyTypeGetDecimal
    {
        public override double Get(Args args)
        {
            return HearManager.Instance.GlobalDin;
        }
        
        public override string String => "Global Din";
    }
}