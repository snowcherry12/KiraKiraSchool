using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using UnityEngine;
using UnityEngine.Audio;

namespace GameCreator.Runtime.Common.Audio
{
    public class AudioBuffer
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private IAudioConfig m_AudioConfig;

        [NonSerialized] private Args m_Args;

        [NonSerialized] private readonly AnimFloat m_Volume = new AnimFloat(1f);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        internal AudioClip AudioClip => this.AudioSource.clip;
        
        internal GameObject Target { get; private set; }
        
        internal AudioSource AudioSource { get; }
        internal Transform Transform { get; }
        
        public float Pitch { get; set; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        internal AudioBuffer(Transform parent, AudioMixerGroup audioMixerGroup)
        {
            GameObject audioBuffer = new GameObject("Audio Source");

            this.Target = null;
            
            this.Transform = audioBuffer.GetComponent<Transform>();
            this.AudioSource = audioBuffer.AddComponent<AudioSource>();
            this.AudioSource.outputAudioMixerGroup = audioMixerGroup;
            
            this.Transform.SetParent(parent);
        }
        
        // UPDATE METHODS: ------------------------------------------------------------------------

        internal bool Update(float volume)
        {
            this.m_Volume.Update();

            volume *= this.m_Volume.Current;
            this.AudioSource.volume = Rescale(volume);

            GameObject target = this.m_AudioConfig?.GetTrackTarget(this.m_Args);
            
            Character character = target.Get<Character>();
            if (character != null && character.Animim.Animator.isHuman)
            {
                target = character.Animim.Animator.GetBoneTransform(HumanBodyBones.Head).gameObject;
            }
            
            if (target != null) this.Transform.position = target.transform.position;
            
            float timeScale = this.m_AudioConfig?.UpdateMode == TimeMode.UpdateMode.GameTime
                ? Time.timeScale
                : 1f;
            
            this.AudioSource.pitch = this.Pitch * timeScale; 

            return this.AudioSource.isPlaying;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        internal async Task Play(AudioClip audioClip, IAudioConfig audioConfig, Args args)
        {
            this.AudioSource.clip = audioClip;
            
            this.m_AudioConfig = audioConfig;
            this.m_Args = args;

            this.Setup();
            
            this.AudioSource.Stop();
            this.AudioSource.Play();

            while (!ApplicationManager.IsExiting && this.AudioSource.isPlaying)
            {
                await Task.Yield();
            }
        }

        internal async Task Stop(float transition)
        {
            this.m_Volume.Target = 0f;
            this.m_Volume.Smooth = transition;
            
            this.AudioSource.SetScheduledEndTime(AudioSettings.dspTime + transition);
            while (!ApplicationManager.IsExiting && this.AudioSource.isPlaying)
            {
                await Task.Yield();
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void Setup()
        {
            float startVolume = this.m_AudioConfig.TransitionIn <= float.Epsilon
                ? this.m_AudioConfig.Volume
                : 0f;

            this.Pitch = this.m_AudioConfig.Pitch;
            this.m_Volume.Current = startVolume;
            this.m_Volume.Target = this.m_AudioConfig.Volume;
            this.m_Volume.Smooth = this.m_AudioConfig.TransitionIn;
            
            this.AudioSource.volume = Rescale(startVolume);
            this.AudioSource.pitch = this.Pitch;
            this.AudioSource.spatialBlend = this.m_AudioConfig.SpatialBlend;

            this.Target = this.m_AudioConfig.GetTrackTarget(this.m_Args);
        }
        
        // UTILITY METHODS: -----------------------------------------------------------------------

        private static float Rescale(float volume)
        {
            return volume * volume;
        }
    }
}