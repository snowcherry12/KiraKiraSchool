using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [AddComponentMenu("")]
    public class LuminanceManager : Singleton<LuminanceManager> 
    {
        private struct LuminanceData
        {
            // PROPERTIES: ------------------------------------------------------------------------
            
            public int Frame { get; }
            public Vector3 Position { get; }
            public float Luminance { get; }
            
            // CONSTRUCTOR: -----------------------------------------------------------------------
            
            public LuminanceData(int frame, Vector3 position, float luminance)
            {
                this.Frame = frame;
                this.Position = position;
                this.Luminance = luminance;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////
        
        private const float TIMEOUT_CLEAR_CACHE = 100f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized]
        private readonly Dictionary<int, Luminance> m_Instances = new Dictionary<int, Luminance>();

        [NonSerialized] 
        private readonly Dictionary<int, LuminanceData> m_Cache = new Dictionary<int, LuminanceData>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public float AmbientIntensity { get; set; }

        public Dictionary<int, Luminance>.ValueCollection List => this.m_Instances.Values;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void OnCreate()
        {
            base.OnCreate();
            
            ScheduleManager.Instance.RunInterval(
                this.ClearCache,
                TIMEOUT_CLEAR_CACHE,
                TimeMode.UpdateMode.UnscaledTime
            );
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Insert(Luminance instance)
        {
            int instanceId = instance.GetInstanceID();
            this.m_Instances[instanceId] = instance;
        }
        
        public void Remove(Luminance instance)
        {
            int instanceId = instance.GetInstanceID();
            this.m_Instances.Remove(instanceId);
        }
        
        public float LuminanceAt(Transform target)
        {
            int targetId = target.GetInstanceID();
            if (this.m_Cache.TryGetValue(targetId, out LuminanceData data))
            {
                if (data.Frame == Time.frameCount && target.position == data.Position)
                {
                    return data.Luminance;
                }
            }
            
            float intensity = this.AmbientIntensity;
            
            foreach (Luminance instance in this.List)
            {
                if (instance == null) continue;
                intensity += instance.IntensityTo(target);
            }
            
            this.m_Cache[targetId] = new LuminanceData(
                Time.frameCount,
                target.position,
                intensity
            );
            
            return intensity;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void ClearCache()
        {
            this.m_Cache.Clear();
        }
    }
}