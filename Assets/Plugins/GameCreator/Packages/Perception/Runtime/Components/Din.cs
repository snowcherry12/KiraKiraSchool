using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.Perception
{
    [Icon(RuntimePaths.PACKAGES + "Perception/Editor/Gizmos/GizmoDin.png")]
    [AddComponentMenu("Game Creator/Perception/Din")]
    
    [Serializable]
    public class Din : MonoBehaviour
    {
        private static readonly Color GIZMOS_COLOR = new Color(1f, 0f, 0f, 0.1f);
        
        private static readonly Easing.Type[] ROLL_OFF =
        {
            Easing.Type.ExpoIn,
            Easing.Type.QuintIn,
            Easing.Type.QuartIn,
            Easing.Type.CubicIn,
            Easing.Type.QuadIn,
            Easing.Type.SineIn,
            Easing.Type.Linear,
            Easing.Type.SineOut,
            Easing.Type.QuadOut,
            Easing.Type.CubicOut,
            Easing.Type.QuartOut,
            Easing.Type.QuintOut,
            Easing.Type.ExpoOut,
        };
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private float m_Radius = 5f;
        [SerializeField] [Range(0, 12)] private int m_RollOff = 3;

        [FormerlySerializedAs("m_UseOccluders")] [SerializeField]
        private EnablerLayerMask m_UseObstruction = new EnablerLayerMask();

        [SerializeField]
        private PropertyGetDecimal m_Level = GetDecimalDecimal.Create(0.75f);

        [SerializeField] private AudioSource m_AudioSource;
        [SerializeField] [Range(0f, 1f)] private float m_Volume = 1f; 
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Args m_Args;

        [NonSerialized] private float m_NextOcclusionVolumeTime;
        [NonSerialized] private AnimFloat m_OcclusionVolume = new AnimFloat(1f, 0.5f);
        
        [NonSerialized] private AudioListener m_AudioListener;

        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
        {
            this.m_Args = new Args(this.gameObject);
        }

        private void OnEnable()
        {
            HearManager.Instance.InsertLocalDin(this);
            if (this.m_AudioSource != null) this.m_AudioSource.Play();
        }

        private void OnDisable()
        {
            if (ApplicationManager.IsExiting) return;
            
            HearManager.Instance.RemoveLocalDin(this);
            if (this.m_AudioSource != null) this.m_AudioSource.Stop();
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        private void Update()
        {
            if (this.m_AudioSource != null)
            {
                Transform audioListener = this.GetAudioListener();
                if (audioListener == null) return;
                
                if (this.m_NextOcclusionVolumeTime >= Time.unscaledTime) return;

                float distance = Vector3.Distance(this.transform.position, audioListener.position);
                float volume = this.m_Volume * (1f - Mathf.Clamp01(distance / this.m_Radius));
                
                this.m_AudioSource.volume = AudioManager.Instance.Volume.Ambient * volume;
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public float GetValue(Perception perception)
        {
            if (perception == null) return 0f;
            if (!this.isActiveAndEnabled) return 0f;
            
            this.m_Args.ChangeTarget(perception);
            
            float distance = Vector3.Distance(
                perception.transform.position,
                this.transform.position
            );
            
            if (distance >= this.m_Radius) return 0f;

            float distanceRatio = Mathf.Clamp01(distance / this.m_Radius);
            Easing.Type rollOff = ROLL_OFF[this.m_RollOff];
            
            float t = Easing.GetEase(rollOff, 1f, 0f, distanceRatio);
            float level = (float) this.m_Level.Get(this.m_Args) * t;

            if (this.m_UseObstruction.IsEnabled)
            {
                level -= Obstruction.GetNoiseDamp(
                    this.transform.position,
                    perception.gameObject,
                    this.m_UseObstruction.Value
                );
            }
            
            return level;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private Transform GetAudioListener()
        {
            if (this.m_AudioListener == null)
            {
                this.m_AudioListener = FindObjectOfType<AudioListener>();
            }

            return this.m_AudioListener != null ? this.m_AudioListener.transform : null;
        }
        
        // GIZMO METHODS: -------------------------------------------------------------------------

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying && !this.isActiveAndEnabled) return;
            
            Gizmos.color = GIZMOS_COLOR;
            const float partitions = 10f;
            
            for (int i = 0; i <= partitions; ++i)
            {
                Easing.Type rollOff = ROLL_OFF[this.m_RollOff];
                float radius = this.m_Radius * Easing.GetEase(rollOff, 1f, 0f, i / partitions);
                
                GizmosExtension.Octahedron(this.transform.position, this.transform.rotation, radius);
            }
        }
    }
}