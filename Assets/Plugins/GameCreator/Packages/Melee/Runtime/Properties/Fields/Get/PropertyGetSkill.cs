using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class PropertyGetSkill : TPropertyGet<PropertyTypeGetSkill, Skill>
    {
        public PropertyGetSkill() : base(new GetSkillInstance())
        { }

        public PropertyGetSkill(PropertyTypeGetSkill defaultType) : base(defaultType)
        { }
    }
}