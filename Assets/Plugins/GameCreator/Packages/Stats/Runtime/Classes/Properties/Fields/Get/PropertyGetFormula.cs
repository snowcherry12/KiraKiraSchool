using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class PropertyGetFormula : TPropertyGet<PropertyTypeGetFormula, Formula>
    {
        public PropertyGetFormula() : base(new GetFormulaInstance())
        { }

        public PropertyGetFormula(PropertyTypeGetFormula defaultType) : base(defaultType)
        { }
    }
}