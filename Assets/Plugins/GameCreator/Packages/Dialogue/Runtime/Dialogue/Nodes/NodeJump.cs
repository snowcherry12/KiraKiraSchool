using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public struct NodeJump
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private JumpType m_Jump;
        [SerializeField] private IdString m_JumpTo;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public JumpType Jump => this.m_Jump;
        
        public IdString JumpTo => this.m_JumpTo;
        
        // STATIC CONSTRUCTORS: -------------------------------------------------------------------

        public static NodeJump Continue()
        {
            return new NodeJump
            {
                m_Jump = JumpType.Continue
            };
        }

        public static NodeJump Exit()
        {
            return new NodeJump
            {
                m_Jump = JumpType.Exit
            };
        }

        public static NodeJump To(IdString tag)
        {
            return new NodeJump
            {
                m_Jump = JumpType.Jump,
                m_JumpTo = tag
            };
        }
    }
}