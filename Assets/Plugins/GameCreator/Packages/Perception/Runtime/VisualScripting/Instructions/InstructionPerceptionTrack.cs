using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Version(0, 0, 1)]
    
    [Title("Track Awareness")]
    [Description("Starts tracking a game object in order to become aware of it")]

    [Category("Perception/Track Awareness")]

    [Keywords("Perceive", "Alert")]
    [Image(typeof(IconTrack), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class InstructionPerceptionTrack : Instruction
    {
        [SerializeField]
        private PropertyGetGameObject m_Perception = GetGameObjectPerception.Create;

        [SerializeField]
        private PropertyGetGameObject m_Track = GetGameObjectPlayer.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"{this.m_Perception} track {this.m_Track}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            Perception perception = this.m_Perception.Get<Perception>(args);
            GameObject track = this.m_Track.Get(args);

            if (perception == null || track == null) return DefaultResult;
            perception.Track(track);
            
            return DefaultResult;
        }
    }
}