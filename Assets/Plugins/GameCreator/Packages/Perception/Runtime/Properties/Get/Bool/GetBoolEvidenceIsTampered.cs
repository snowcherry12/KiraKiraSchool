using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Title("Is Evidence Tampered")]
    [Category("Perception/Is Evidence Tampered")]
    
    [Image(typeof(IconEvidenceTamper), ColorTheme.Type.Red)]
    [Description("Returns true if the Evidence game object has been tampered")]
    
    [Serializable]
    public class GetBoolEvidenceIsTampered : PropertyTypeGetBool
    {
        [SerializeField]
        private PropertyGetGameObject m_Evidence = GetGameObjectEvidence.Create;

        public override bool Get(Args args)
        {
            Evidence evidence = this.m_Evidence.Get<Evidence>(args);
            return evidence != null && evidence.IsTampered;
        }

        public static PropertyGetBool Create => new PropertyGetBool(
            new GetBoolEvidenceIsTampered()
        );
        
        public override string String => $"{this.m_Evidence} Is Tampered";

        public override bool EditorValue => false;
    }
}