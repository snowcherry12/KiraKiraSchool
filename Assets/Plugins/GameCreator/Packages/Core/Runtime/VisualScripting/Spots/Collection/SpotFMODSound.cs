using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.Audio;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Play FMOD Audio")]
    [Keywords("Audio", "Sounds", "FMOD")]
    [Image(typeof(IconMusicNote), ColorTheme.Type.Yellow)]
    
    [Category("Audio/Play FMOD Audio")]
    [Description(
        "Plays a User Interface sound effect when the Hotspot is activated or deactivated"
    )]

    [Serializable]
    public class SpotFMODSound : Spot
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] protected PropertyGetFMODAudio m_OnActivate = new PropertyGetFMODAudio();
        [SerializeField] protected PropertyGetFMODAudio m_OnDeactivate = new PropertyGetFMODAudio();

        [SerializeField]
        private AudioConfigSoundUI m_AudioSettings = new AudioConfigSoundUI();

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private bool m_WasActive;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Play {this.m_OnActivate} / {this.m_OnDeactivate}";

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override void OnEnable(Hotspot hotspot)
        {
            base.OnEnable(hotspot);
            this.m_WasActive = false;
        }

        public override void OnDisable(Hotspot hotspot)
        {
            base.OnDisable(hotspot);
            if (ApplicationManager.IsExiting) return;
            
            if (this.m_WasActive)
            {
                Args args = new Args(hotspot.gameObject, hotspot.Target);
                FMODAudio fmodAudio = this.m_OnDeactivate.Get(args);
                
                if (fmodAudio != null)
                {
                    _ = AudioManager.Instance.UserInterface.Play(
                        fmodAudio,
                        this.m_AudioSettings,
                        args
                    );
                }
            }
        }

        public override void OnUpdate(Hotspot hotspot)
        {
            base.OnUpdate(hotspot);

            switch (this.m_WasActive)
            {
                case false when hotspot.IsActive:
                {
                    Args args = new Args(hotspot.gameObject, hotspot.Target);
                    FMODAudio fmodAudio = this.m_OnActivate.Get(args);
                    if (fmodAudio != null)
                    {
                        _ = AudioManager.Instance.UserInterface.Play(
                            fmodAudio,
                            this.m_AudioSettings,
                            args
                        );
                    }

                    break;
                }
                case true when !hotspot.IsActive:
                {
                    Args args = new Args(hotspot.gameObject, hotspot.Target);
                    FMODAudio fmodAudio = this.m_OnDeactivate.Get(args);
                    if (fmodAudio != null)
                    {
                        _ = AudioManager.Instance.UserInterface.Play(
                            fmodAudio,
                            this.m_AudioSettings,
                            args
                        );
                    }

                    break;
                }
            }


            this.m_WasActive = hotspot.IsActive;
        }
    }
}