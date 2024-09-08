using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public class PropertySetFormula : TPropertySet<PropertyTypeSetFormula, Formula>
    {
        public PropertySetFormula() : base(new SetFormulaNone())
        { }

        public PropertySetFormula(PropertyTypeSetFormula defaultType) : base(defaultType)
        { }
    }
}