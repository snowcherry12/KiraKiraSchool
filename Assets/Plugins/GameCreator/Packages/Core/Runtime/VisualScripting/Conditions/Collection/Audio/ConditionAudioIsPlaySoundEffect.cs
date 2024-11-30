using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Is Sound Effect Playing")]
    [Description("Returns true if the given sound effect is playing")]

    [Category("Audio/Is Sound Effect Playing")]
    
    [Parameter("Audio Clip", "The audio clip to check")]

    [Keywords("SFX", "Music", "Audio", "Running")]
    [Image(typeof(IconMusicNote), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionAudioIsPlaySoundEffect : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetAudio m_AudioClip = new PropertyGetAudio();
        [SerializeField] private PropertyGetFMODAudio m_FMODAudio = new PropertyGetFMODAudio();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => string.Format(
            "is SFX {0}{1}{2}{3} playing",
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
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            AudioClip audioClip = this.m_AudioClip.Get(args);
            FMODAudio fmodAudio = this.m_FMODAudio.Get(args);
            return audioClip != null && AudioManager.Instance.SoundEffect.IsPlaying(audioClip)
                || fmodAudio != null && AudioManager.Instance.SoundEffect.IsPlaying(fmodAudio);
        }
    }
}
