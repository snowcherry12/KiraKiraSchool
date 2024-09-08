using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Skill value of a Global List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetSkillGlobalList : PropertyTypeGetSkill
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueSkill.TYPE_ID);

        public override Skill Get(Args args) => this.m_Variable.Get<Skill>(args);

        public override string String => this.m_Variable.ToString();
    }
}