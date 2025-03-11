using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class ClipReloadMagazine : Clip
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Prefab = GetGameObjectInstance.Create();
        [SerializeField] private HandleField m_StartOn = new HandleField(HumanBodyBones.LeftHand);

        [SerializeField] private PropertyGetGameObject m_CompleteOn = GetGameObjectTarget.Create();
        [SerializeField] private Vector3 m_LocalPosition = Vector3.zero;
        [SerializeField] private Vector3 m_LocalRotation = Vector3.zero;
        
        [SerializeField] private float m_Transition = 0.25f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private bool m_DoneStart;
        [NonSerialized] private bool m_DoneComplete;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ClipReloadMagazine() : this(DEFAULT_TIME - DEFAULT_PAD, DEFAULT_DURATION + DEFAULT_PAD * 2f)
        { }

        public ClipReloadMagazine(float time) : base(time, 0f)
        { }
        
        public ClipReloadMagazine(float time, float duration) : base(time, duration)
        { }
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------

        protected override void OnReset(ITrack track, Args args)
        {
            base.OnReset(track, args);
            this.m_DoneStart = false;
            this.m_DoneComplete = false;
            
            Character character = args.Self.Get<Character>();
            if (character == null) return;

            Animator animator = character.Animim.Animator;
            ShooterStance stance = character.Combat.RequestStance<ShooterStance>();

            stance.Reloading.PreviousMagazine = stance.Reloading.CurrentMagazine; 
            
            GameObject prefab = this.m_Prefab.Get(args);
            HandleResult handle = this.m_StartOn.Get(args);
            
            if (prefab == null) return;
            bool prefabWasActive = prefab.activeSelf;
            prefab.SetActive(false);
            
            GameObject magazine = UnityEngine.Object.Instantiate(
                prefab,
                handle.Bone.GetTransform(animator)
            );
            
            prefab.SetActive(prefabWasActive);
            stance.Reloading.CurrentMagazine = magazine;
            
            magazine.transform.localPosition = handle.LocalPosition;
            magazine.transform.localRotation = handle.LocalRotation;
        }

        protected override void OnStart(ITrack track, Args args)
        {
            base.OnStart(track, args);
            this.RunStart(args);
        }

        protected override void OnComplete(ITrack track, Args args)
        {
            base.OnComplete(track, args);
            this.RunFinish(args);
        }

        protected override void OnCancel(ITrack track, Args args)
        {
            base.OnCancel(track, args);
            
            if (this.m_DoneStart == false) this.RunStart(args);
            if (this.m_DoneComplete == false) this.RunFinish(args);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RunStart(Args args)
        {
            this.m_DoneStart = true;
            
            Character character = args.Self.Get<Character>();
            if (character == null) return;

            ShooterStance stance = character.Combat.RequestStance<ShooterStance>();
            if (stance.Reloading.CurrentMagazine != null)
            {
                stance.Reloading.CurrentMagazine.SetActive(true);
            }
        }

        private void RunFinish(Args args)
        {
            this.m_DoneComplete = true;
            
            Character character = args.Self.Get<Character>();
            if (character == null) return;
            
            ShooterStance stance = character.Combat.RequestStance<ShooterStance>();
            if (stance.Reloading.CurrentMagazine == null) return;
            
            GameObject parent = this.m_CompleteOn.Get(args);
            if (parent == null) return;
            
            stance.Reloading.CurrentMagazine.transform.SetParent(parent.transform, true);
            stance.Reloading.CurrentMagazine.Require<Well>().Run(
                this.m_LocalPosition,
                Quaternion.Euler(this.m_LocalRotation),
                character.Time,
                this.m_Transition
            );
        }
    }
}