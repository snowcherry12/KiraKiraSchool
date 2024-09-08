using System;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Characters;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Melee
{
    [Title("Soft Lock")]
    [Category("Soft Lock")]
    [Image(typeof(IconBullsEye), ColorTheme.Type.Blue)]
    
    [Description("Follows the mid point between the character and the targeted one while allowing to orbit")]
    
    [Serializable]
    public class ShotTypeSoftLock : TShotTypeLook
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private ShotSystemSoftLock m_ShotSystemSoftLock;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly Transform[] m_Ignore = new Transform[2];

        // PROPERTIES: ----------------------------------------------------------------------------

        public override Transform[] Ignore
        {
            get
            {
                Transform transform = this.Look.GetLookTarget(this);
                Character character = transform != null ? transform.Get<Character>() : null;
                
                if (transform == null || character == null)
                {
                    this.m_Ignore[0] = null;
                    this.m_Ignore[1] = null;
                }
                else
                {
                    this.m_Ignore[0] = transform;
                    this.m_Ignore[1] = character.Combat.Targets.Primary != null
                        ? character.Combat.Targets.Primary.transform
                        : null;
                }

                return this.m_Ignore;
            }
        }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ShotTypeSoftLock()
        {
            this.m_ShotSystemLook = new ShotSystemLook(
                GetGameObjectPlayer.Create(),
                GetDirectionMeleeCharacterTarget.Create
            );
            
            this.m_ShotSystemSoftLock = new ShotSystemSoftLock();
            
            this.m_ShotSystems.Add(this.m_ShotSystemLook.Id, this.m_ShotSystemLook);
            this.m_ShotSystems.Add(this.m_ShotSystemSoftLock.Id, this.m_ShotSystemSoftLock);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void AddRotation(float pitch, float yaw)
        {
            this.m_ShotSystemSoftLock.Pitch += pitch;
            this.m_ShotSystemSoftLock.Yaw += yaw;
        }

        // OVERRIDERS: ----------------------------------------------------------------------------

        protected override void OnBeforeAwake(ShotCamera shotCamera)
        {
            base.OnBeforeAwake(shotCamera);
            this.m_ShotSystemSoftLock?.OnAwake(this);
        }

        protected override void OnBeforeStart(ShotCamera shotCamera)
        {
            base.OnBeforeStart(shotCamera);
            this.m_ShotSystemSoftLock?.OnStart(this);
        }

        protected override void OnBeforeDestroy(ShotCamera shotCamera)
        {
            base.OnBeforeDestroy(shotCamera);
            this.m_ShotSystemSoftLock?.OnDestroy(this);
        }

        protected override void OnBeforeEnable(TCamera camera)
        {
            base.OnBeforeEnable(camera);
            this.m_ShotSystemSoftLock?.OnEnable(this, camera);
        }

        protected override void OnBeforeDisable(TCamera camera)
        {
            base.OnBeforeDisable(camera);
            this.m_ShotSystemSoftLock?.OnDisable(this, camera);
        }
        
        protected override void OnBeforeUpdate()
        {
            base.OnBeforeUpdate();
            this.m_ShotSystemSoftLock?.OnUpdate(this);
        }
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        public override void DrawGizmos(Transform transform)
        {
            base.DrawGizmos(transform);
            
            if (!Application.isPlaying) return;
            this.m_ShotSystemSoftLock.OnDrawGizmos(this, transform);
        }

        public override void DrawGizmosSelected(Transform transform)
        {
            base.DrawGizmosSelected(transform);
            
            if (!Application.isPlaying) return;
            this.m_ShotSystemSoftLock.OnDrawGizmosSelected(this, transform);
        }
    }
}