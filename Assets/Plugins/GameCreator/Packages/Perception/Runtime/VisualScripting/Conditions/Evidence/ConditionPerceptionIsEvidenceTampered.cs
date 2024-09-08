using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Is Evidence Tampered")]
    [Description("Determines whether an Evidence game object has been tampered or not")]

    [Category("Perception/Evidence/Is Evidence Tampered")]
    
    [Parameter("Evidence", "The Evidence component")]

    [Keywords("Notice", "Change", "Tamper", "Modify", "Fiddle")]
    [Image(typeof(IconEvidenceTamper), ColorTheme.Type.Red)]
    
    [Serializable]
    public class ConditionPerceptionIsEvidenceTampered : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Evidence = GetGameObjectEvidence.Create;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"{this.m_Evidence} is Tampered";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            Evidence evidence = this.m_Evidence.Get<Evidence>(args);
            return evidence != null && evidence.IsTampered;
        }
    }
}
