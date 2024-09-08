using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Perception
{
    [Serializable]
    public class SensorList : TPolymorphicList<TSensor>
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeReference] private TSensor[] m_Sensors =
        {
            new SensorSee(),
            new SensorFeel(),
            new SensorHear(),
            new SensorSmell(),
        };

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override int Length => this.m_Sensors.Length;
        
        [field: NonSerialized] private Perception Perception { get; set; }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public T Get<T>() where T : TSensor
        {
            foreach (TSensor sensor in this.m_Sensors)
            {
                if (!sensor.IsEnabled) continue;
                if (sensor is T sensorType) return sensorType;
            }
            
            return null;
        }
        
        // INITIALIZERS: --------------------------------------------------------------------------

        public void Enable(Perception perception)
        {
            this.Perception = perception;
            foreach (TSensor sensor in this.m_Sensors)
            {
                sensor.Enable(perception);
            }
        }

        public void Disable(Perception perception)
        {
            this.Perception = perception;
            foreach (TSensor sensor in this.m_Sensors)
            {
                sensor.Disable(perception);
            }
        }

        // UPDATE METHODS: ------------------------------------------------------------------------
        
        public void Update()
        {
            foreach (TSensor sense in this.m_Sensors)
            {
                sense.Update();
            }
        }

        public void FixedUpdate()
        {
            foreach (TSensor sense in this.m_Sensors)
            {
                sense.FixedUpdate();
            }
        }
        
        // GIZMOS: --------------------------------------------------------------------------------

        public void DrawGizmos(Perception perception)
        {
            foreach (TSensor sense in this.m_Sensors)
            {
                sense.DrawGizmos(perception);
            }
        }
    }
}