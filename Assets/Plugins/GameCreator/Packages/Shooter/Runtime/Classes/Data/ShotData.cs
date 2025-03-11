using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    public struct ShotData
    {
        public static ShooterWeapon LastShooterWeapon { get; private set; }
        public static GameObject LastProp { get; private set; }
        public static Vector3 LastShooterPosition { get; private set; }
        public static Vector3 LastShooterDirection { get; private set; }
        public static Vector3 LastHitPosition { get; private set; }
        public static float LastChargeRatio { get; private set; }
        public static float LastDistance { get; private set; }
        public static int LastNumPierces { get; private set; }
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public Character Source { get; }
        [field: NonSerialized] public GameObject Target { get; private set; }
        
        [field: NonSerialized] public ShooterWeapon Weapon { get; }
        [field: NonSerialized] public IdString SightId { get; }
        
        [field: NonSerialized] public GameObject Prop { get; }
        [field: NonSerialized] public GameObject Projectile { get; private set; }

        [field: NonSerialized] public Vector3 ShootPosition { get; }
        [field: NonSerialized] public Vector3 ShootDirection { get; }
        [field: NonSerialized] public Vector3 HitPoint { get; private set; }
        [field: NonSerialized] public MaterialSoundsAsset ImpactSound { get; private set; }
        [field: NonSerialized] public PropertyGetInstantiate ImpactEffect { get; private set; }
        
        [field: NonSerialized] public int Cartridges { get; }
        [field: NonSerialized] public float ChargeRatio { get; }
        [field: NonSerialized] public float Delay { get; }
        [field: NonSerialized] public float Distance { get; private set; }
        [field: NonSerialized] public float PullTime { get; }
        [field: NonSerialized] public int Pierces { get; private set; }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ShotData(Character source,
            ShooterWeapon weapon,
            IdString sightId,
            GameObject prop,
            Vector3 shootPosition,
            Vector3 shootDirection,
            MaterialSoundsAsset impactSound,
            PropertyGetInstantiate impactEffect,
            int cartridges,
            float chargeRatio,
            float delay,
            float pullTime)
        {
            this.Source = source;
            this.Target = null;

            this.Weapon = weapon;
            this.SightId = sightId;
            
            this.Prop = prop;
            this.Projectile = null;
        
            this.ShootPosition = shootPosition;
            this.ShootDirection = shootDirection;
            this.HitPoint = default;
            this.ImpactSound = impactSound;
            this.ImpactEffect = impactEffect;

            this.Cartridges = cartridges;
            this.ChargeRatio = chargeRatio;
            this.Delay = delay;
            this.Distance = 0f;
            this.PullTime = pullTime;
            this.Pierces = 0;

            LastShooterWeapon = weapon;
            LastProp = prop;
            LastShooterPosition = shootPosition;
            LastShooterDirection = shootDirection;
            LastChargeRatio = chargeRatio;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void UpdateProjectile(GameObject projectile)
        {
            this.Projectile = projectile;
        }
        
        public void UpdateHit(GameObject target, Vector3 hitPoint, float distance, int pierces)
        {
            this.Target = target;
            this.HitPoint = hitPoint;
            
            this.Distance = distance;
            this.Pierces = pierces;

            LastHitPosition = hitPoint;
            LastDistance = distance;
            LastNumPierces = pierces;
        }
    }
}