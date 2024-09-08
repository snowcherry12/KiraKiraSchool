using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Version(0, 0, 1)]
    
    [Title("Untrack Awareness")]
    [Description("Stops tracking a game object and forgets about it")]

    [Category("Perception/Untrack Awareness")]

    [Keywords("Perceive", "Alert")]
    [Image(typeof(IconTrack), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionPerceptionUntrack : Instruction
    {
        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        private PropertyGetGameObject m_Untrack = GetGameObjectPlayer.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"{this.m_Perception} untrack {this.m_Untrack}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            GameObject untrack = this.m_Untrack.Get(args);

            if (perception == null || untrack == null) return DefaultResult;
            perception.Untrack(untrack);
            
            return DefaultResult;
        }
    }
}