using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Title("Last Formula Result")]
    [Category("Stats/Last Formula Result")]

    [Image(typeof(IconFormula), ColorTheme.Type.Purple, typeof(OverlayArrowLeft))]
    [Description("Returns the last value computed by a Formula")]

    [Serializable]
    public class GetDecimalLastFormulaResult : PropertyTypeGetDecimal
    {
        public override double Get(Args args)
        {
            return Formula.LastFormulaResult;
        }

        public static PropertyGetDecimal Create => new PropertyGetDecimal(
            new GetDecimalLastFormulaResult()
        );

        public override string String => "Last Formula";
    }
}