using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Skill value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetSkillLocalList : PropertyTypeGetSkill
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueSkill.TYPE_ID);

        public override Skill Get(Args args) => this.m_Variable.Get<Skill>(args);

        public override string String => this.m_Variable.ToString();
    }
}