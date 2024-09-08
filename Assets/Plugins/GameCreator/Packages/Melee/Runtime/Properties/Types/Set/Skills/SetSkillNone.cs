using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("None")]
    [Category("None")]
    [Description("Don't save on anything")]
    
    [Image(typeof(IconNull), ColorTheme.Type.TextLight)]

    [Serializable]
    public class SetSkillNone : PropertyTypeSetSkill
    {
        public override void Set(Skill value, Args args)
        { }
        
        public override void Set(Skill value, GameObject gameObject)
        { }

        public static PropertySetSkill Create => new PropertySetSkill(
            new SetSkillNone()
        );

        public override string String => "(none)";
    }
}