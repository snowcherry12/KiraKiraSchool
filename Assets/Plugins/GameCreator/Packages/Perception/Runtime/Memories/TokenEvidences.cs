using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Serializable]
    public class TokenEvidences : Token
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private string[] m_List;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public string[] List => this.m_List;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TokenEvidences(Perception perception)
        {
            if (perception == null)
            {
                this.m_List = Array.Empty<string>();
                return;
            }

            List<string> evidences = new List<string>();
            foreach (string evidenceTag in perception.EvidenceTags)
            {
                if (!perception.GetEvidence(evidenceTag)) continue;
                evidences.Add(evidenceTag);
            }

            this.m_List = evidences.ToArray();
        }
    }
}