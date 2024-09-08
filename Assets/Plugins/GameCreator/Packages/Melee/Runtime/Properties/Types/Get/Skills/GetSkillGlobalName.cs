using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Skill value of a Global Name Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetSkillGlobalName : PropertyTypeGetSkill
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueSkill.TYPE_ID);

        public override Skill Get(Args args) => this.m_Variable.Get<Skill>(args);

        public override string String => this.m_Variable.ToString();
    }
}