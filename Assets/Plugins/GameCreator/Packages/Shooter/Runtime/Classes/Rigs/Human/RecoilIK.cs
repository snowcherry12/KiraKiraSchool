using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    public class RecoilIK
    {
        private const float DECAY_SAFE = 0.001f;
        
        private const float MAX = 1f;
        private const float MIN = 0f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private float m_ImpactDecay;
        [NonSerialized] private float m_RecoverDecay;
        
        [NonSerialized] private readonly SpringFloat m_Impact = new SpringFloat(MIN, DECAY_SAFE);
        [NonSerialized] private readonly SpringFloat m_Recover = new SpringFloat(MIN, DECAY_SAFE);

        [NonSerialized] private readonly SpringVector3 m_Position = new SpringVector3(DECAY_SAFE);
        [NonSerialized] private readonly SpringVector3 m_Rotation = new SpringVector3(DECAY_SAFE);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public float Value => this.m_Impact.Current;
        
        public Vector3 Position => new Vector3(
            this.m_Position.Current.x * this.m_Impact.Current,
            this.m_Position.Current.y * this.m_Impact.Current,
            this.m_Position.Current.z * this.m_Impact.Current
        );
        
        public Vector3 Rotation => new Vector3(
            this.m_Rotation.Current.x * this.m_Impact.Current,
            this.m_Rotation.Current.y * this.m_Impact.Current,
            this.m_Rotation.Current.z * this.m_Impact.Current
        );
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void OnShoot(Vector2 position, Vector3 rotation, float kickback, float impactDecay, float recoverDecay)
        {
            this.m_Position.Target = new Vector3(position.x, position.y, kickback);
            this.m_Rotation.Target = rotation;
            
            this.m_Recover.Current = MAX;
            
            this.m_ImpactDecay = impactDecay;
            this.m_RecoverDecay = recoverDecay;
        }
        
        public void Update(float deltaTime)
        {
            this.m_Position.Update(this.m_ImpactDecay, deltaTime);
            this.m_Rotation.Update(this.m_ImpactDecay, deltaTime);
            
            this.m_Recover.Update(MIN, this.m_RecoverDecay, deltaTime);
            this.m_Impact.Update(this.m_Recover.Current, DECAY_SAFE, deltaTime);
        }
    }
}