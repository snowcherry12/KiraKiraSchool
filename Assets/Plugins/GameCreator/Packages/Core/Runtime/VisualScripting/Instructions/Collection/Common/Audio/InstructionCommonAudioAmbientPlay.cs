using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.Audio;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Play Ambient")]
    [Description(
        "Plays a looped Audio Clip. Useful for background effects or persistent sounds."
    )]

    [Category("Audio/Play Ambient")]
    
    [Parameter("Audio Clip", "The Audio Clip to be played")]
    [Parameter("Transition In", "Time it takes for the sound to fade in")]
    [Parameter("Spatial Blending", "Whether the sound is placed in a 3D space or not")]
    [Parameter("Target", "A Game Object reference that the sound follows as the source")]

    [Keywords("Audio", "Ambience", "Background")]
    [Image(typeof(IconBird), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class InstructionCommonAudioAmbientPlay : Instruction
    {
        [SerializeField] private PropertyGetAudio m_AudioClip = GetAudioNone.Create;
        [SerializeField] private PropertyGetFMODAudio m_FMODAudio = GetFMODAudioNone.Create;
        [SerializeField] private AudioConfigAmbient m_Config = new AudioConfigAmbient();

        public override string Title => $"Play Ambient: {this.m_AudioClip} (or) {this.m_FMODAudio}";

        protected override Task Run(Args args)
        {
            AudioClip audioClip = this.m_AudioClip.Get(args);
            FMODAudio fmodAudio = this.m_FMODAudio.Get(args);
            if (audioClip == null && fmodAudio == null) return DefaultResult;
            
            if (!AudioManager.Instance.Ambient.IsPlaying(audioClip))
            {
                _ = AudioManager.Instance.Ambient.Play(
                    audioClip, 
                    this.m_Config,
                    args
                );   
            }
            if (!AudioManager.Instance.Ambient.IsPlaying(fmodAudio))
            {
                _ = AudioManager.Instance.Ambient.Play(
                    fmodAudio, 
                    this.m_Config,
                    args
                );   
            }

            return DefaultResult;
        }
    }
}