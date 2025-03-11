using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class SightItem : TPolymorphicItem<SightItem>
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private IdString m_Id = new IdString("my-sight-id");

        [SerializeField] private Sight m_Sight;

        [SerializeField] private bool m_ScopeThrough;
        [SerializeField] private Vector3 m_ScopePosition = Vector3.up * 0.25f;
        [SerializeField] private Vector3 m_ScopeRotation = Vector3.zero;
        [SerializeField] private float m_ScopeDistance = 0.25f;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public IdString Id => this.m_Id;
        
        public override string Title
        {
            get
            {
                string sight = this.m_Sight != null ? this.m_Sight.name : "(none)";
                return $"{this.m_Id.String}: {sight}";
            }
        }

        public Sight Sight => this.m_Sight;

        public bool ScopeThrough => this.m_ScopeThrough;

        public Vector3 ScopePosition => this.m_ScopePosition;
        public Quaternion ScopeRotation => Quaternion.Euler(this.m_ScopeRotation);
        public float ScopeDistance => this.m_ScopeDistance;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool CanShoot(Args args)
        {
            return this.m_Sight != null && this.m_Sight.CanShoot(args);
        }
        
        public void Enter(Character character, ShooterWeapon weapon)
        {
            if (this.m_Sight == null) return;
            this.m_Sight.Enter(character, weapon);
        }
        
        public void Exit(Character character, ShooterWeapon weapon)
        {
            if (this.m_Sight == null) return;
            this.m_Sight.Exit(character, weapon);
        }
    }
}