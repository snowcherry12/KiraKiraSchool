using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Is UI Playing")]
    [Description("Returns true if the given UI sound is playing")]

    [Category("Audio/Is UI Playing")]
    
    [Parameter("Audio Clip", "The audio clip to check")]

    [Keywords("SFX", "Music", "Audio", "Running")]
    [Image(typeof(IconUIButton), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionAudioIsPlayUI : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetAudio m_AudioClip = new PropertyGetAudio();
        [SerializeField] private PropertyGetFMODAudio m_FMODAudio = new PropertyGetFMODAudio();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => $"is UI {this.m_AudioClip} (or) {this.m_FMODAudio} playing";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            AudioClip audioClip = this.m_AudioClip.Get(args);
            FMODAudio fmodAudio = this.m_FMODAudio.Get(args);
            return audioClip != null && AudioManager.Instance.UserInterface.IsPlaying(audioClip)
                || fmodAudio != null && AudioManager.Instance.UserInterface.IsPlaying(fmodAudio);
        }
    }
}
