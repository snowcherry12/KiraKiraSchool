using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    public class SpringVector3
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] private SpringFloat X { get; set; }
        [field: NonSerialized] private SpringFloat Y { get; set; }
        [field: NonSerialized] private SpringFloat Z { get; set; }

        public Vector3 Current
        {
            get => new Vector3(this.X.Current, this.Y.Current, this.Z.Current);
            set
            {
                this.X.Current = value.x;
                this.Y.Current = value.y;
                this.Z.Current = value.z;
            }
        }
        
        public Vector3 Target
        {
            get => new Vector3(this.X.Target, this.Y.Target, this.Z.Target);
            set
            {
                this.X.Target = value.x;
                this.Y.Target = value.y;
                this.Z.Target = value.z;
            }
        }

        public float Decay
        {
            get => this.X.Decay;
            set
            {
                this.X.Decay = value;
                this.Y.Decay = value;
                this.Z.Decay = value;
            }
        }
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public SpringVector3(float decay = SpringFloat.DEFAULT_DECAY) : this(Vector3.zero, decay)
        { }
        
        public SpringVector3(Vector3 value, float decay = SpringFloat.DEFAULT_DECAY)
        {
            this.X = new SpringFloat(value.x, decay);
            this.Y = new SpringFloat(value.y, decay);
            this.Z = new SpringFloat(value.z, decay);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Update(float deltaTime)
        {
            this.X.Update(deltaTime);
            this.Y.Update(deltaTime);
            this.Z.Update(deltaTime);
        }
        
        public void Update(float decay, float deltaTime)
        {
            this.Decay = decay;
            this.Update(deltaTime);
        }
        
        public void Update(Vector3 target, float decay, float deltaTime)
        {
            this.Target = target;
            this.Update(decay, deltaTime);
        }
    }
}