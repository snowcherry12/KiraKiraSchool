using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Stop Ambient")]
    [Description("Stops a currently playing Ambient audio")]

    [Category("Audio/Stop Ambient")]
    
    [Parameter("Audio Clip", "The Audio Clip to be played")]
    [Parameter("Wait To Complete", "Check if you want to wait until the sound has faded out")]
    [Parameter("Transition Out", "Time it takes for the sound to fade out")]

    [Keywords("Audio", "Ambience", "Background", "Fade", "Mute")]
    [Image(typeof(IconBird), ColorTheme.Type.TextLight, typeof(OverlayCross))]
    
    [Serializable]
    public class InstructionCommonAudioAmbientStop : Instruction
    {
        [SerializeField] private PropertyGetAudio m_AudioClip = GetAudioNone.Create;
        [SerializeField] private PropertyGetFMODAudio m_FMODAudio = GetFMODAudioNone.Create;
        
        [SerializeField] private bool m_WaitToComplete = false;
        [SerializeField] private float transitionOut = 2f;

        public override string Title => string.Format(
            "Stop Ambient: {0}{1}{2}{3} {4}",
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
                : string.Empty,
            this.transitionOut < float.Epsilon 
                ? string.Empty 
                : string.Format(
                    "in {0} second{1}", 
                    this.transitionOut,
                    Mathf.Approximately(this.transitionOut, 1f) ? string.Empty : "s"
                )
        );

        protected override async Task Run(Args args)
        {
            AudioClip audioClip = this.m_AudioClip.Get(args);
            FMODAudio fmodAudio = this.m_FMODAudio.Get(args);
            if (audioClip == null && fmodAudio == null) return;
            
            if (this.m_WaitToComplete)
            {
                if (audioClip != null) await AudioManager.Instance.Ambient.Stop(audioClip, this.transitionOut);
                if (fmodAudio != null) await AudioManager.Instance.Ambient.Stop(fmodAudio, this.transitionOut);
            }
            else
            {
                if (audioClip != null) _ = AudioManager.Instance.Ambient.Stop(audioClip, this.transitionOut);
                if (fmodAudio != null) _ = AudioManager.Instance.Ambient.Stop(fmodAudio, this.transitionOut);
            }
        }
    }
}