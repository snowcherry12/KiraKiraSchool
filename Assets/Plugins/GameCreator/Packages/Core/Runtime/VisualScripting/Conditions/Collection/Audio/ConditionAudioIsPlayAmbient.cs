using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Is Ambient Playing")]
    [Description("Returns true if the given Ambient sound is playing")]

    [Category("Audio/Is Ambient Playing")]
    
    [Parameter("Audio Clip", "The audio clip to check")]

    [Keywords("SFX", "Music", "Audio", "Running")]
    [Image(typeof(IconBird), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionAudioIsPlayAmbient : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetAudio m_AudioClip = new PropertyGetAudio();
        [SerializeField] private PropertyGetFMODAudio m_FMODAudio = new PropertyGetFMODAudio();

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Summary => string.Format(
            "is Ambient {0}{1}{2}{3} playing",
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
            return audioClip != null && AudioManager.Instance.Ambient.IsPlaying(audioClip)
                || fmodAudio != null && AudioManager.Instance.Ambient.IsPlaying(fmodAudio);
        }
    }
}
