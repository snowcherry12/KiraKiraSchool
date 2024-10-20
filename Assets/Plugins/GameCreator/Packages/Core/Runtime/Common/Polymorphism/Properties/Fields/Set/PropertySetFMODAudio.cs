using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class PropertySetFMODAudio : TPropertySet<PropertyTypeSetFMODAudio, FMODAudio>
    {
        public PropertySetFMODAudio() : base(new SetFMODAudioNone())
        { }

        public PropertySetFMODAudio(PropertyTypeSetFMODAudio defaultType) : base(defaultType)
        { }
    }
}