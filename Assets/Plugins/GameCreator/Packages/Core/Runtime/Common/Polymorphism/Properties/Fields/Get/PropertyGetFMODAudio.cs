using System;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class PropertyGetFMODAudio : TPropertyGet<PropertyTypeGetFMODAudio, FMODAudio>
    {
        public PropertyGetFMODAudio() : base(new GetFMODAudio())
        { }

        public PropertyGetFMODAudio(PropertyTypeGetFMODAudio defaultType) : base(defaultType)
        { }

        public PropertyGetFMODAudio(FMODAudio fmodAudio) : base(new GetFMODAudio(fmodAudio))
        { }
    }
}