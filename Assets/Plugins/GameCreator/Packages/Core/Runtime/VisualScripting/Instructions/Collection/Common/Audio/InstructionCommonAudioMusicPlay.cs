using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.Audio;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Play Music")]
    [Description(
        "Plays a looped Audio Clip. Useful for background music or persistent sounds."
    )]

    [Category("Audio/Play Music")]
    
    [Parameter("Audio Clip", "The Audio Clip to be played")]
    [Parameter("Transition In", "Time it takes for the sound to fade in")]
    [Parameter("Spatial Blending", "Whether the sound is placed in a 3D space or not")]
    [Parameter("Target", "A Game Object reference that the sound follows as the source")]

    [Keywords("Audio", "Ambience", "Background")]
    [Image(typeof(IconHeadset), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class InstructionCommonAudioMusicPlay : Instruction
    {
        [SerializeField] private PropertyGetAudio m_AudioClip = GetAudioNone.Create;
        [SerializeField] private PropertyGetFMODAudio m_FMODAudio = GetFMODAudioNone.Create;
        [SerializeField] private AudioConfigMusic m_Config = new AudioConfigMusic();

        public override string Title => string.Format(
            "Play Music: {0}{1}{2}{3}",
            this.m_AudioClip.ToString() != "None"
                ? this.m_AudioClip
                : string.Empty,
            this.m_AudioClip.ToString() != "None" && this.m_FMODAudio.ToString() != "None"
                ? " (&) "
                : string.Empty,
            this.m_AudioClip.ToString() == "None" && this.m_FMODAudio.ToString() == "None"
                ? "None"
                : string.Empty,
            this.m_FMODAudio.ToString() != "None"
                ? this.m_FMODAudio
                : string.Empty
        );

        protected override Task Run(Args args)
        {
            AudioClip audioClip = this.m_AudioClip.Get(args);
            FMODAudio fmodAudio = this.m_FMODAudio.Get(args);
            if (audioClip == null && fmodAudio == null) return DefaultResult;
            
            if (!AudioManager.Instance.Music.IsPlaying(audioClip))
            {
                _ = AudioManager.Instance.Music.Play(
                    audioClip, 
                    this.m_Config,
                    args
                );   
            }
            if (!AudioManager.Instance.Music.IsPlaying(fmodAudio))
            {
                _ = AudioManager.Instance.Music.Play(
                    fmodAudio, 
                    this.m_Config,
                    args
                );   
            }
            return DefaultResult;
        }
    }
}