using System;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    public class AttackSkill : TAttackState
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private float m_StartTime;
        [NonSerialized] private CancelMeleeSequence m_Cancel;
        
        [NonSerialized] private AttackDuration m_Duration;
        [NonSerialized] private AttackSpeed m_Speed;

        [NonSerialized] private MeleePhase m_PreviousPhase;

        [NonSerialized] private bool m_StartWarping;
        [NonSerialized] private bool m_FinishWarping;
        [NonSerialized] private Location m_StartLocationSelf;
        [NonSerialized] private Location m_StartLocationTarget;

        [NonSerialized] private Easing.Type m_WarpEasing;
        [NonSerialized] private Location m_WarpLocationSelf;
        [NonSerialized] private Location m_WarpLocationTarget;

        [NonSerialized] private Args m_Args;
        
        [NonSerialized] private readonly HashSet<int> m_HitsBuffer = new HashSet<int>();
        [NonSerialized] private readonly List<Striker> m_Strikers = new List<Striker>();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override MeleePhase Phase
        {
            get
            {
                float time = this.CurrentTime - this.m_StartTime;
                float total = this.m_Duration.Total;
                
                float ratio = total > 0f ? time / total : 1f;
                AttackRatio ratios = this.ComboSkill.GetPhasesDilatedRatios(this.m_Speed);
                
                if (ratios is { Strike: <= 0f, Recovery: <= 0f })
                {
                    return ratio <= 1f
                        ? MeleePhase.Anticipation
                        : MeleePhase.None;
                }

                if (ratio <= ratios.Anticipation) return MeleePhase.Anticipation;
                if (ratio <= ratios.Anticipation + ratios.Strike) return MeleePhase.Strike;
                
                float exitTime = Math.Max(
                    this.m_Duration.Total - this.ComboSkill.TransitionOut,
                    this.m_Duration.Anticipation + this.m_Duration.Strike
                );

                return time < exitTime ? MeleePhase.Recovery : MeleePhase.None;
            }
        }

        private bool CanCancel
        {
            get
            {
                float time = this.CurrentTime - this.m_StartTime;
                float ratio = this.m_Duration.Total > 0f ? time / this.m_Duration.Total : 1f;

                return this.ComboSkill.CanCancel(ratio, this.m_Speed);
            }
        }

        // EVENTS: --------------------------------------------------------------------------------
        
        public event Action EventAnticipation;
        public event Action EventStrike;
        public event Action EventRecovery;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public AttackSkill(Attacks attacks) : base(attacks)
        { }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override bool TryToCancel()
        {
            float time = this.CurrentTime - this.m_StartTime;
            float ratio = this.m_Duration.Total > 0f ? time / this.m_Duration.Total : 1f;

            bool canCancel = this.ComboSkill.CanCancel(ratio, this.m_Speed);

            if (!canCancel) return false;
            
            this.Attacks.ToNone();
            return true;
        }
        
        public override void ForceCancel()
        {
            this.Attacks.ToNone();
        }

        public void ClearHits()
        {
            this.m_HitsBuffer.Clear();
        }
        
        // TRANSITION METHODS: --------------------------------------------------------------------

        protected override void WhenEnter(IStateMachine stateMachine)
        {
            base.WhenEnter(stateMachine);

            this.m_StartTime = this.CurrentTime;
            this.m_Cancel = new CancelMeleeSequence();
            
            this.m_HitsBuffer.Clear();
            this.m_Strikers.Clear();
            
            this.m_Args = this.Attacks.MeleeStance.Args.Clone;
            
            Character self = this.Attacks.MeleeStance.Character;

            self.Combat.Block.LowerGuard();
            self.Busy.SetBusy();
            self.Driver.SetGravityInfluence(GRAVITY_INFLUENCE_KEY, this.ComboSkill.Gravity);
            
            this.m_StartWarping = true;
            this.m_FinishWarping = true;

            float poise = this.ComboSkill.GetPoiseArmor(this.m_Args);
            self.Combat.Poise.Reset(poise);

            this.m_Speed = this.ComboSkill.GetSpeed(this.m_Args);
            this.m_Duration = this.ComboSkill.GetDuration(this.m_Speed, this.m_Args);

            this.ComboSkill.Run(
                self, 
                this.ComboSkill.Animation != null ? this.ComboSkill.Animation.length : 0f,
                this.m_Speed,
                this.m_Cancel,
                this.m_Args
            );

            if (this.ComboSkill.Motion == MeleeMotion.MotionWarp)
            {
                if (this.ComboSkill.SyncReaction != null)
                {
                    Character enemy = this.m_Args.ComponentFromTarget<Character>();
                    if (enemy != null)
                    {
                        MeleeStance enemyMelee = enemy.Combat.RequestStance<MeleeStance>();

                        enemyMelee.PlayReaction(
                            self.gameObject,
                            new ReactionInput(Vector3.zero, this.ComboSkill.GetPower(this.m_Args)),
                            this.ComboSkill.SyncReaction,
                            true
                        );
                    }
                }
            }
            
            this.EventAnticipation?.Invoke();
        }

        protected override void WhenExit(IStateMachine stateMachine)
        {
            base.WhenExit(stateMachine);
            
            foreach (Striker striker in this.m_Strikers)
            {
                if (striker == null) continue;
                striker.OnStop();
            }

            Character self = this.Attacks.MeleeStance.Character;
            
            self.Busy.SetAvailable();
            self.CanUseRootMotionPosition = true;
            self.CanUseRootMotionRotation = true;
            
            self.Driver.RemoveGravityInfluence(GRAVITY_INFLUENCE_KEY);

            this.ComboSkill.Stop(
                self, this.m_Cancel,
                this.m_Args
            );
        }

        // UPDATE METHOD: -------------------------------------------------------------------------

        protected override void WhenUpdate(IStateMachine stateMachine)
        {
            base.WhenUpdate(stateMachine);
            
            Character character = this.Attacks.MeleeStance.Character;

            float elapsedTime = this.CurrentTime - this.m_StartTime;
            float elapsedRatio = elapsedTime / this.m_Duration.Total;

            switch (this.ComboSkill.Motion)
            {
                case MeleeMotion.None: break;
                case MeleeMotion.RootMotion: this.UpdateRootMotion(character, elapsedRatio); break;
                case MeleeMotion.MotionWarp: this.UpdateMotionWarp(character, elapsedRatio); break;
                
                default: throw new ArgumentOutOfRangeException();
            }
            
            MeleePhase currentPhase = this.Phase;
            if (currentPhase != this.m_PreviousPhase)
            {
                this.m_PreviousPhase = currentPhase;

                switch (currentPhase)
                {
                    case MeleePhase.Reaction:
                    case MeleePhase.Charge:
                    case MeleePhase.Anticipation:
                        break;
                
                    case MeleePhase.Strike:
                        this.OnEnterPhaseStrike();
                        break;
                
                    case MeleePhase.Recovery:
                        this.OnEnterPhaseRecovery();
                        break;
                
                    case MeleePhase.None:
                        this.OnEnterPhaseFinish();
                        return;
                
                    default: throw new ArgumentOutOfRangeException();
                }
            }

            switch (currentPhase)
            {
                case MeleePhase.Strike: 
                    this.OnUpdatePhaseStrike();
                    break;
                
                case MeleePhase.Recovery:
                    if (this.CanCancel) this.UpdateInput(); 
                    break;
                
                case MeleePhase.None:
                case MeleePhase.Charge:
                case MeleePhase.Anticipation:
                case MeleePhase.Reaction:
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnUpdatePhaseStrike()
        {
            List<StrikeOutput> hits = new List<StrikeOutput>();
            int predictions = Math.Max(this.ComboSkill.Strike.Predictions, 1);
            
            foreach (Striker striker in this.m_Strikers)
            {
                if (striker == null) continue;
                
                List<StrikeOutput> candidates = striker.OnUpdate(predictions);
                foreach (StrikeOutput candidate in candidates)
                {
                    Transform character = this.Attacks.MeleeStance.Character.transform;
                    if (candidate.GameObject.transform.IsChildOf(character)) continue;
                    
                    if (this.m_HitsBuffer.Contains(candidate.GameObject.GetInstanceID())) continue;
                    if (hits.Contains(candidate)) continue;
                    
                    hits.Add(candidate);
                }
            }

            foreach (StrikeOutput hit in hits)
            {
                Args hitArgs = new Args(
                    this.Attacks.MeleeStance.Character.gameObject,
                    hit.GameObject
                );

                if (!this.ComboSkill.CanHit(hitArgs))
                {
                    this.m_HitsBuffer.Add(hit.GameObject.GetInstanceID());
                    continue;
                }
                
                Trigger[] triggers = hit.GameObject.GetComponents<Trigger>();
                foreach (Trigger trigger in triggers)
                {
                    CommandArgs commandArgs = new CommandArgs(
                        EventMeleeHit.COMMAND_HIT,
                        this.Attacks.MeleeStance.Character.gameObject
                    );
                    
                    trigger.OnReceiveCommand(commandArgs);
                }

                Transform attacker = this.Attacks.MeleeStance.Character.transform;

                Transform hitTransform = hit.GameObject.transform;
                Character hitCharacter = hit.GameObject.Get<Character>();

                Vector3 skillDirection = this.ComboSkill.Strike.Direction switch
                {
                    MeleeDirection.None => Vector3.zero,
                    MeleeDirection.Left => attacker.TransformDirection(Vector3.left),
                    MeleeDirection.Right => attacker.TransformDirection(Vector3.right),
                    MeleeDirection.Forward => attacker.TransformDirection(Vector3.forward),
                    MeleeDirection.Backwards => attacker.TransformDirection(Vector3.back),
                    MeleeDirection.Upwards => attacker.TransformDirection(Vector3.up),
                    MeleeDirection.Downwards => attacker.TransformDirection(Vector3.down),
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                Vector3 direction = hitTransform.InverseTransformDirection(skillDirection);
                float power = this.ComboSkill.GetPower(hitArgs);

                ShieldInput shieldInput = new ShieldInput(direction, hit.Point, power);
                ReactionInput reactionInput = new ReactionInput(direction, power);

                if (hitCharacter == null || hitCharacter.IsDead)
                {
                    CanHit canHit = hit.GameObject.Get<CanHit>();
                    if (canHit != null && canHit.AllowHits(this.Attacks.MeleeStance.Character))
                    {
                        this.ComboSkill.OnHit(hitArgs, hit.Point, direction);
                    }
                    
                    this.m_HitsBuffer.Add(hit.GameObject.GetInstanceID());
                    continue;
                }

                if (hitCharacter.Dash.IsDodge)
                {
                    hitCharacter.Dash.OnDodge(hitArgs);
                }
                else if (!hitCharacter.Combat.Invincibility.IsInvincible)
                {
                    MeleeStance hitMeleeStance = hitCharacter.Combat.RequestStance<MeleeStance>();
                    
                    IShield shield = hitCharacter.Combat.GetBlock(
                        shieldInput,
                        hitArgs,
                        this.ComboSkill.CanBlock(hitArgs),
                        this.ComboSkill.CanParry(hitArgs),
                        out ShieldOutput shieldOutput
                    );
                    
                    switch (shieldOutput.Type)
                    {
                        case BlockType.Break:
                        case BlockType.None: this.ComboSkill.OnHit(hitArgs, hit.Point, direction); break;
                        case BlockType.Block: this.ComboSkill.OnBlocked(hitArgs, reactionInput); break;
                        case BlockType.Parry: this.ComboSkill.OnParried(hitArgs, reactionInput); break;
                        default: throw new ArgumentOutOfRangeException();
                    }
                    
                    if (shield != null)
                    {
                        Args blockArgs = new Args(hitArgs.Target, hitArgs.Self);
                        shield.OnDefend(blockArgs, shieldOutput, reactionInput);
                    }
                    else
                    {
                        hitMeleeStance.Hit(
                            this.Attacks.MeleeStance.Character,
                            reactionInput, 
                            this.ComboSkill
                        );
                    }
                }
                
                this.m_HitsBuffer.Add(hit.GameObject.GetInstanceID());
            }
        }
        
        private void OnEnterPhaseStrike()
        {
            this.ComboSkill.OnStrike(this.m_Args);
            
            this.Attacks.MeleeStance.Character.Gestures.SetSpeed(
                this.ComboSkill.Animation, 
                this.m_Speed.Strike
            );

            if (this.Attacks.Weapon != null)
            {
                GameObject prop = this.Attacks.MeleeStance.Character.Combat.GetProp(this.Attacks.Weapon);
                
                this.m_Strikers.Clear();
                
                Striker[] candidates = prop != null
                    ? prop.GetComponentsInChildren<Striker>()
                    : this.Attacks.MeleeStance.Character.GetComponentsInChildren<Striker>();
                
                foreach (Striker candidate in candidates)
                {
                    if (!this.ComboSkill.Strike.UseStriker(candidate)) continue;
                    this.m_Strikers.Add(candidate);
                }
            }

            foreach (Striker striker in this.m_Strikers)
            {
                if (striker == null) continue;
                striker.OnBegin(this.Attacks.MeleeStance.Character, this.ComboSkill);
            }

            this.EventStrike?.Invoke();
        }
        
        private void OnEnterPhaseRecovery()
        {
            this.Attacks.MeleeStance.Character.Gestures.SetSpeed(
                this.ComboSkill.Animation, 
                this.m_Speed.Recovery
            );
            
            foreach (Striker striker in this.m_Strikers)
            {
                if (striker == null) continue;
                striker.OnStop();
            }
                
            this.EventRecovery?.Invoke();
        }

        private void OnEnterPhaseFinish()
        {
            this.Attacks.ToNone();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void UpdateInput()
        {
            if (this.Attacks.Input.HasChargeInQueue)
            {
                int comboId = this.Attacks.ComboId;
                this.UpdateCharge(comboId);
            }
            
            if (this.Attacks.Input.HasExecuteInQueue)
            {
                int comboId = this.Attacks.ComboId;
                this.UpdateExecute(comboId, comboId);
            }
        }

        private void UpdateRootMotion(Character self, float elapsedRatio)
        {
            bool motionPosition = this.ComboSkill.CanUseRootMotionPosition(elapsedRatio, this.m_Speed);
            bool motionRotation = this.ComboSkill.CanUseRootMotionRotation(elapsedRatio, this.m_Speed);
            
            self.CanUseRootMotionPosition = motionPosition;
            self.CanUseRootMotionRotation = motionRotation;
        }

        private void UpdateMotionWarp(Character self, float elapsedRatio)
        {
            float motionWarpRatio = this.ComboSkill.MotionWarpingRatio(elapsedRatio, this.m_Speed, this.m_Args);
            Character target = this.m_Args.ComponentFromTarget<Character>();

            if (motionWarpRatio < 0f)
            {
                if (!this.m_StartWarping && this.m_FinishWarping)
                {
                    motionWarpRatio = 1f;
                    this.m_FinishWarping = false;
                }
                else
                {
                    if (target != null)
                    {
                        target.CanUseRootMotionPosition = true;
                        target.CanUseRootMotionRotation = true;
                    }
                    
                    self.CanUseRootMotionPosition = true;
                    self.CanUseRootMotionRotation = true;
                    
                    return;       
                }
            }

            if (this.m_StartWarping)
            {
                this.m_StartLocationSelf = new Location(self.transform.position, self.transform.rotation);
                this.m_StartLocationTarget = new Location(
                    target != null ? target.transform.position : this.m_Args.Target != null
                        ? this.m_Args.Target.transform.position
                        : Vector3.zero,
                    target != null 
                        ? target.transform.rotation
                        : Quaternion.identity
                );
                
                ClipMeleeMotionWarping motionWarp = this.ComboSkill.GetMotionWarp();
                
                if (motionWarp != null)
                {
                    this.m_WarpEasing = motionWarp.Easing;
                    this.m_WarpLocationSelf = motionWarp.GetLocationSelf(this.m_Args);
                    this.m_WarpLocationTarget = motionWarp.GetLocationTarget(this.m_Args);
                }
                else
                {
                    this.m_WarpEasing = Easing.Type.Linear;
                    this.m_WarpLocationSelf = Location.None;
                    this.m_WarpLocationTarget = Location.None;
                }

                this.m_StartWarping = false;
            }

            float motionWarpRatioEase = Easing.GetEase(this.m_WarpEasing, 0f, 1f, motionWarpRatio);

            this.UpdateWarpLocation(
                self, 
                this.m_StartLocationSelf, 
                this.m_WarpLocationSelf, 
                motionWarpRatioEase
            );
            
            this.UpdateWarpLocation(
                target, 
                this.m_StartLocationTarget,
                this.m_WarpLocationTarget,
                motionWarpRatioEase
            );
        }

        // PRIVATE UTILITY METHODS: ---------------------------------------------------------------

        private void UpdateWarpLocation(Character character, Location start, Location warp, float ratio)
        {
            if (character == null) return;
            
            if (warp.HasPosition(character.gameObject))
            {
                character.CanUseRootMotionPosition = false;
                Vector3 feetOffset = character.transform.position - character.Feet;
                
                Vector3 nextPosition = Vector3.Lerp(
                    start.GetPosition(character.gameObject) - feetOffset,
                    warp.GetPosition(character.gameObject) - feetOffset,
                    ratio
                );

                Vector3 currentPosition = character.Feet;
                character.Driver.AddPosition(nextPosition - currentPosition);
            }

            if (warp.HasRotation(character.gameObject))
            {
                character.CanUseRootMotionRotation = false;
                Quaternion nextRotation = Quaternion.Lerp(
                    start.GetRotation(character.gameObject),
                    warp.GetRotation(character.gameObject),
                    ratio
                );
                
                character.Driver.SetRotation(nextRotation);
            }
        }
    }
}