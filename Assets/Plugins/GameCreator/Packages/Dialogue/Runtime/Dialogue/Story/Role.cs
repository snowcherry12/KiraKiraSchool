using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class Role
    {
        public const string NAME_ACTOR = nameof(m_Actor);
        public const string NAME_TARGET = nameof(m_Target);
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private Actor m_Actor;
        [SerializeField] private PropertyGetGameObject m_Target;
            
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public Actor Actor => this.m_Actor;
        public PropertyGetGameObject Target => this.m_Target;

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public Role()
        {
            this.m_Target = new PropertyGetGameObject();
        }
            
        public Role(Actor actor) : this()
        {
            this.m_Actor = actor;
        }
    }
}