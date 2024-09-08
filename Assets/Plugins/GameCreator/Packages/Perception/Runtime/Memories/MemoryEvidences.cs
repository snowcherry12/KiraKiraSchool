using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Image(typeof(IconEvidence), ColorTheme.Type.Green)]
    
    [Title("Evidences")]
    [Category("Perception/Evidences")]
    
    [Description("Remembers which tampered Evidence objects have already been noticed")]
    
    [Serializable]
    public class MemoryEvidences : Memory
    {
        public override string Title => "Evidences";

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override Token GetToken(GameObject target)
        {
            Perception perception = target.Get<Perception>();
            return new TokenEvidences(perception);
        }

        public override void OnRemember(GameObject target, Token token)
        {
            Perception perception = target.Get<Perception>();
            if (perception == null) return;

            if (token is not TokenEvidences tokenEvidences) return;

            foreach (string evidenceTag in tokenEvidences.List)
            {
                perception.SetEvidence(evidenceTag);
            }
        }
    }
}