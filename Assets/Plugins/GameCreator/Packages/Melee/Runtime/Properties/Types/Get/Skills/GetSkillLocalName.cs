using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.Melee
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]

    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]
    [Description("Returns the Skill value of a Local Name Variable")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetSkillLocalName : PropertyTypeGetSkill
    {
        [SerializeField]
        protected FieldGetLocalName m_Variable = new FieldGetLocalName(ValueSkill.TYPE_ID);

        public override Skill Get(Args args) => this.m_Variable.Get<Skill>(args);

        public override string String => this.m_Variable.ToString();
    }
}