using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.Audio;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Play UI sound")]
    [Description("Plays a non-diegetic user interface Audio Clip")]

    [Category("Audio/Play UI sound")]
    
    [Parameter("Audio Clip", "The Audio Clip to be played")]
    [Parameter("Wait To Complete", "Check if you want to wait until the sound finishes")]
    [Parameter("Pitch", "A random pitch value ranging between two values")]
    [Parameter("Spatial Blending", "Whether the sound is placed in a 3D space or not")]
    [Parameter("Target", "A Game Object reference that the sound follows as its source")]

    [Keywords("Audio", "Sounds", "User", "Interface", "Beep", "Button")]
    [Image(typeof(IconUIButton), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class InstructionCommonAudioUIPlay : Instruction
    {
        [SerializeField] private PropertyGetAudio m_AudioClip = GetAudioNone.Create;
        [SerializeField] private PropertyGetFMODAudio m_FMODAudio = GetFMODAudioNone.Create;
        [SerializeField] private bool m_WaitToComplete = false;
        
        [SerializeField] private AudioConfigSoundUI m_Config = new AudioConfigSoundUI();

        public override string Title => $"Play UI sound: {this.m_AudioClip} (or) {this.m_FMODAudio}";

        protected override async Task Run(Args args)
        {
            AudioClip audioClip = this.m_AudioClip.Get(args);
            FMODAudio fmodAudio = this.m_FMODAudio.Get(args);
            if (audioClip == null && fmodAudio == null) return;
            
            if (this.m_WaitToComplete)
            {
                if (audioClip != null) await AudioManager.Instance.UserInterface.Play(audioClip, this.m_Config, args);
                if (fmodAudio != null) await AudioManager.Instance.UserInterface.Play(fmodAudio, this.m_Config, args);
            }
            else
            {
                if (audioClip != null) _ = AudioManager.Instance.UserInterface.Play(audioClip, this.m_Config, args);
                if (fmodAudio != null) _ = AudioManager.Instance.UserInterface.Play(fmodAudio, this.m_Config, args);
            }
        }
    }
}