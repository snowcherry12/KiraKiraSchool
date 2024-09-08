using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Compare Luminance At")]
    [Description("Compares the Luminance value with another value")]

    [Category("Perception/See/Compare Luminance At")]
    
    [Parameter("Target", "The object reference that checks its Luminance")]
    [Parameter("Value", "The comparison to the Luminance value")]

    [Keywords("Light", "Dim", "Lit", "Expose", "Sun")]
    [Image(typeof(IconLuminance), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class ConditionPerceptionLuminance : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        [SerializeField]
        private CompareDouble m_Value = new CompareDouble();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Target} Luminance {this.m_Value}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            GameObject target = this.m_Target.Get(args);
            if (target == null) return false;

            float luminance = LuminanceManager.Instance.LuminanceAt(target.transform);
            return this.m_Value.Match(luminance, args);
        }
    }
}
