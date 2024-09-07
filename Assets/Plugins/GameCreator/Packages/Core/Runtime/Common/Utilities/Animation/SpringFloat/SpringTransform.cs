using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    public class SpringTransform
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public SpringVector3 Position { get; private set; }
        [field: NonSerialized] public SpringQuaternion Rotation { get; private set; }
        [field: NonSerialized] public SpringVector3 Scale { get; private set; }

        public float Decay
        {
            get => this.Position.Decay;
            set
            {
                this.Position.Decay = value;
                this.Rotation.Decay = value;
                this.Scale.Decay = value;
            }
        }
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public SpringTransform(
            Vector3 position,
            Quaternion rotation,
            Vector3 scale,
            float decay = SpringFloat.DEFAULT_DECAY)
        {
            this.Position = new SpringVector3(position, decay);
            this.Rotation = new SpringQuaternion(rotation, decay);
            this.Scale = new SpringVector3(scale, decay);
        }
        
        public SpringTransform(
            Transform transform,
            bool inLocalSpace = false,
            float decay = SpringFloat.DEFAULT_DECAY)
        {
            if (inLocalSpace)
            {
                this.Position = new SpringVector3(transform.localPosition, decay);
                this.Rotation = new SpringQuaternion(transform.localRotation, decay);
                this.Scale = new SpringVector3(transform.localScale, decay);
            }
            else
            {
                this.Position = new SpringVector3(transform.position, decay);
                this.Rotation = new SpringQuaternion(transform.rotation, decay);
                this.Scale = new SpringVector3(transform.lossyScale, decay);
            }
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Update(float deltaTime)
        {
            this.Position.Update(deltaTime);
            this.Rotation.Update(deltaTime);
            this.Scale.Update(deltaTime);
        }
        
        public void Update(float decay, float deltaTime)
        {
            this.Position.Update(decay, deltaTime);
            this.Rotation.Update(decay, deltaTime);
            this.Scale.Update(decay, deltaTime);
        }
        
        public void Update(Vector3 position, Quaternion rotation, float decay, float deltaTime)
        {
            this.Position.Update(position, decay, deltaTime);
            this.Rotation.Update(rotation, decay, deltaTime);
            this.Scale.Update(decay, deltaTime);
        }
    }
}