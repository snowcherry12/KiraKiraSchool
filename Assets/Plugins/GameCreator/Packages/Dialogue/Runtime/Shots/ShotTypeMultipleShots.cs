using System;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("Multiple Shots")]
    [Category("Multiple Shots")]
    [Image(typeof(IconCameraShot), ColorTheme.Type.Blue, typeof(OverlayPlus))]

    [Description("Switches between multiple Camera Shots")]

    [Serializable]
    public class ShotTypeMultipleShots : TShotType
    {
        [SerializeField] private ShotSystemSwitcher m_Switcher;
        [SerializeField] private ShotSystemNoise m_Noise;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override Args Args
        {
            get
            {
                this.m_Args ??= new Args(this.m_ShotCamera);

                ShotCamera shot = this.m_Switcher.CurrentShot;
                return shot != null ? shot.ShotType.Args : this.m_Args;
            }
        }

        public override Transform[] Ignore
        {
            get
            {
                ShotCamera shot = this.m_Switcher.CurrentShot;
                return shot != null ? shot.ShotType.Ignore : Array.Empty<Transform>();
            }
        }

        public override bool UseSmoothPosition
        {
            get
            {
                ShotCamera shot = this.m_Switcher.CurrentShot;
                return shot != null && shot.ShotType.UseSmoothPosition;
            }
        }

        public override bool UseSmoothRotation
        {
            get
            {
                ShotCamera shot = this.m_Switcher.CurrentShot;
                return shot != null && shot.ShotType.UseSmoothRotation;
            }
        }

        public override bool HasTarget => this.m_Switcher.CurrentShot != null;

        public override Vector3 Target
        {
            get
            {
                ShotCamera shot = this.m_Switcher.CurrentShot;
                return shot != null ? shot.ShotType.Target : default;
            }
        }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ShotTypeMultipleShots()
        {
            this.m_Switcher = new ShotSystemSwitcher();
            this.m_Noise = new ShotSystemNoise();
            
            this.m_ShotSystems.Add(this.m_Switcher.Id, this.m_Switcher);
            this.m_ShotSystems.Add(this.m_Noise.Id, this.m_Noise);
        }

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        protected override void OnBeforeAwake(ShotCamera shotCamera)
        {
            base.OnBeforeAwake(shotCamera);
            this.m_Switcher?.OnAwake(this);
            this.m_Noise?.OnAwake(this);
        }

        protected override void OnBeforeStart(ShotCamera shotCamera)
        {
            base.OnBeforeStart(shotCamera);
            this.m_Switcher?.OnStart(this);
            this.m_Noise?.OnStart(this);
        }

        protected override void OnBeforeDestroy(ShotCamera shotCamera)
        {
            base.OnBeforeDestroy(shotCamera);
            this.m_Switcher?.OnDestroy(this);
            this.m_Noise?.OnDestroy(this);
        }

        protected override void OnBeforeEnable(TCamera camera)
        {
            base.OnBeforeEnable(camera);
            this.m_Switcher?.OnEnable(this, camera);
            this.m_Noise?.OnEnable(this, camera);
        }

        protected override void OnBeforeDisable(TCamera camera)
        {
            base.OnBeforeDisable(camera);

            this.m_Switcher?.OnDisable(this, camera);
            this.m_Noise?.OnDisable(this, camera);
        }
        
        protected override void OnBeforeUpdate()
        {
            base.OnBeforeUpdate();

            this.m_Switcher?.OnUpdate(this);
            this.m_Noise?.OnUpdate(this);
        }
    }
}