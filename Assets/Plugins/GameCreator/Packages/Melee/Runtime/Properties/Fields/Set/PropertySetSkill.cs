using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class PropertySetSkill : TPropertySet<PropertyTypeSetSkill, Skill>
    {
        public PropertySetSkill() : base(new SetSkillNone())
        { }

        public PropertySetSkill(PropertyTypeSetSkill defaultType) : base(defaultType)
        { }
    }
}