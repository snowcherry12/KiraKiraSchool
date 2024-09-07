using System;
using Unity.Mathematics;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    public class SpringPose
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public SpringVector3 Position { get; }
        [field: NonSerialized] public SpringQuaternion Rotation { get; }

        public float Decay
        {
            get => this.Position.Decay;
            set
            {
                this.Position.Decay = value;
                this.Rotation.Decay = value;
            }
        }
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public SpringPose(float decay = SpringFloat.DEFAULT_DECAY) 
            : this(Vector3.zero, quaternion.identity, decay)
        { }
        
        public SpringPose(
            Vector3 position,
            Quaternion rotation,
            float decay = SpringFloat.DEFAULT_DECAY)
        {
            this.Position = new SpringVector3(position, decay);
            this.Rotation = new SpringQuaternion(rotation, decay);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Update(float deltaTime)
        {
            this.Position.Update(deltaTime);
            this.Rotation.Update(deltaTime);
        }
        
        public void Update(float decay, float deltaTime)
        {
            this.Position.Update(decay, deltaTime);
            this.Rotation.Update(decay, deltaTime);
        }
        
        public void Update(Vector3 position, Quaternion rotation, float decay, float deltaTime)
        {
            this.Position.Update(position, decay, deltaTime);
            this.Rotation.Update(rotation, decay, deltaTime);
        }
    }
}