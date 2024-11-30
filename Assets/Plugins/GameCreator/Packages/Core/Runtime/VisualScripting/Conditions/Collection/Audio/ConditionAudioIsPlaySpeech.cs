using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Is Speech Playing")]
    [Description("Returns true if the given Speech sound is playing")]

    [Category("Audio/Is Speech Playing")]
    
    [Parameter("Audio Clip", "The audio clip to check")]

    [Keywords("SFX", "Music", "Audio", "Running")]
    [Image(typeof(IconFace), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class ConditionAudioIsPlaySpeech : Condition
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetAudio m_AudioClip = GetAudioNone.Create;
        [SerializeField] private PropertyGetFMODAudio m_FMODAudio = GetFMODAudioNone.Create;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Summary => string.Format(
            "is Speech {0}{1}{2}{3} playing",
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
            return audioClip != null && AudioManager.Instance.Speech.IsPlaying(audioClip)
                || fmodAudio != null && AudioManager.Instance.Speech.IsPlaying(fmodAudio);
        }
    }
}
