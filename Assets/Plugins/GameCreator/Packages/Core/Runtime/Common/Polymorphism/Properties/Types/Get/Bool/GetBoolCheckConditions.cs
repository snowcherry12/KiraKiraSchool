using System;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Check Conditions")]
    [Category("Visual Scripting/Check Conditions")]
    
    [Image(typeof(IconCondition), ColorTheme.Type.Green)]
    [Description("Returns whether the conditions list ran successfully or not")]
    
    [Keywords("Conditions", "Check")]
    [Serializable]
    public class GetBoolCheckConditions : PropertyTypeGetBool
    {
        [SerializeField] private RunConditionsList m_Conditions = new RunConditionsList();
        
        public override bool Get(Args args) => this.m_Conditions.Check(args);

        public GetBoolCheckConditions()
        { }

        public GetBoolCheckConditions(params Condition[] conditions)
        {
            this.m_Conditions = new RunConditionsList(conditions);
        }

        public static PropertyGetBool Create()
        {
            return new PropertyGetBool(
                new GetBoolCheckConditions()
            );
        }
        
        public static PropertyGetBool Create(params Condition[] conditions)
        {
            return new PropertyGetBool(
                new GetBoolCheckConditions(conditions)
            );
        }

        public override string String => this.m_Conditions.ToString();
    }
}