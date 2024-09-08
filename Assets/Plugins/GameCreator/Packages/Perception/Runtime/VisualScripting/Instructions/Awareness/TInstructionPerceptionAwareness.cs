using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Parameter("Perception", "The Perception component that changes its awareness")]
    [Parameter("Target", "The target game object that changes its awareness")]

    [Keywords("Know", "Detect", "Alert", "See")]
    
    [Serializable]
    public abstract class TInstructionPerceptionAwareness : Instruction
    {
        [SerializeField]
        protected PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        protected PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();
    }
}