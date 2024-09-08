using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class Expressing
    {
        [SerializeField] private Actor m_Actor;
        [SerializeField] private int m_Expression;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Actor Actor => this.m_Actor;
        
        public int Expression => this.m_Expression;

        public Sprite GetSprite(Args args)
        {
            if (this.m_Actor == null) return null;
            return this.m_Expression < this.m_Actor.ExpressionsLength 
                ? this.m_Actor.GetExpressionFromIndex(this.m_Expression)?.GetSprite(args)
                : null;
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString()
        {
            if (this.m_Actor == null) return "(none)";
            if (this.m_Expression >= this.m_Actor.ExpressionsLength) return "(none)";

            Expression expression = this.m_Actor.GetExpressionFromIndex(this.m_Expression);
            return $"{this.m_Actor.name}[{expression?.Id.String}]";
        }
    }
}