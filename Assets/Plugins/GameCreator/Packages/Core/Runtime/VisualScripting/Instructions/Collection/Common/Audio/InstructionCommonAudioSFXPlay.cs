using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.Audio;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Play Sound Effect")]
    [Description("Plays an Audio Clip sound effect just once")]

    [Category("Audio/Play Sound Effect")]
    
    [Parameter("Audio Clip", "The Audio Clip to be played")]
    [Parameter("Wait To Complete", "Check if you want to wait until the sound finishes")]
    [Parameter("Pitch", "A random pitch value ranging between two values")]
    [Parameter("Transition In", "Time it takes for the sound to fade in")]
    [Parameter("Spatial Blending", "Whether the sound is placed in a 3D space or not")]
    [Parameter("Target", "A Game Object reference that the sound follows as its source")]

    [Keywords("Audio", "Sounds", "SFX", "FX")]
    [Image(typeof(IconMusicNote), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class InstructionCommonAudioSFXPlay : Instruction
    {
        [SerializeField] private PropertyGetAudio m_AudioClip = GetAudioNone.Create;
        [SerializeField] private PropertyGetFMODAudio m_FMODAudio = GetFMODAudioNone.Create;
        [SerializeField] private bool m_WaitToComplete;
        
        [SerializeField] private AudioConfigSoundEffect m_Config = new AudioConfigSoundEffect();

        public override string Title => $"Play SFX: {this.m_AudioClip} (or) {this.m_FMODAudio}";

        protected override async Task Run(Args args)
        {
            AudioClip audioClip = this.m_AudioClip.Get(args);
            FMODAudio fmodAudio = this.m_FMODAudio.Get(args);
            if (audioClip == null && fmodAudio == null) return;
            
            if (this.m_WaitToComplete)
            {
                if (audioClip != null) await AudioManager.Instance.SoundEffect.Play(audioClip, this.m_Config, args);
                if (fmodAudio != null) await AudioManager.Instance.SoundEffect.Play(fmodAudio, this.m_Config, args);
            }
            else
            {
                if (audioClip != null) _ = AudioManager.Instance.SoundEffect.Play(audioClip, this.m_Config, args);
                if (fmodAudio != null) _ = AudioManager.Instance.SoundEffect.Play(fmodAudio, this.m_Config, args);
            }
        }
    }
}
