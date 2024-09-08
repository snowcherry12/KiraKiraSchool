using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Melee
{
    public class AttackReaction : TAttackState
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private float m_StartTime;
        [NonSerialized] private Args m_Args;

        [NonSerialized] private Reaction m_CurrentReaction;
        [NonSerialized] private float m_CurrentDuration;

        [NonSerialized] private float m_CancelTime;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override MeleePhase Phase => MeleePhase.Reaction;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public AttackReaction(Attacks attacks) : base(attacks)
        { }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override bool TryToCancel()
        {
            float currentTime = this.Attacks.MeleeStance.Character.Time.Time;
            if (this.m_StartTime + this.m_CancelTime > currentTime) return false;
            
            this.Attacks.ToNone();
            return true;
        }
        
        public override void ForceCancel()
        {
            this.Attacks.ToNone();
        }
        
        // TRANSITION METHODS: --------------------------------------------------------------------

        protected override void WhenEnter(IStateMachine stateMachine)
        {
            base.WhenEnter(stateMachine);

            this.m_StartTime = this.CurrentTime;
            this.m_Args = this.Attacks.MeleeStance.Args.Clone;
            
            Character self = this.Attacks.MeleeStance.Character;
            
            self.Busy.SetBusy();

            ReactionOutput output = this.Attacks.MeleeStance.Character.Combat.GetHitReaction(
                this.Attacks.ReactionInput,
                this.m_Args,
                this.Attacks.ReactionAsset
            );

            if (output.Reaction == null)
            {
                this.Attacks.ToNone();
                return;
            }

            float duration = output.Length / output.Speed;
            
            this.m_CancelTime = output.CancelTime;
            this.m_CurrentDuration = Math.Max(duration - output.Reaction.TransitionOut, 0f);
            this.m_CurrentReaction = output.Reaction;
            
            self.Driver.SetGravityInfluence(GRAVITY_INFLUENCE_KEY, output.Gravity);
        }

        protected override void WhenExit(IStateMachine stateMachine)
        {
            base.WhenExit(stateMachine);

            Character self = this.Attacks.MeleeStance.Character;
            
            self.Busy.SetAvailable();
            self.Driver.RemoveGravityInfluence(GRAVITY_INFLUENCE_KEY);

            if (this.m_CurrentReaction != null)
            {
                float transition = this.m_CurrentReaction.TransitionOut;
                self.Gestures.Stop(0f, transition);
            }
        }

        // UPDATE METHOD: -------------------------------------------------------------------------

        protected override void WhenUpdate(IStateMachine stateMachine)
        {
            base.WhenUpdate(stateMachine);
            
            float elapsedTime = this.CurrentTime - this.m_StartTime;
            if (elapsedTime > this.m_CurrentDuration) this.Attacks.ToNone();
        }
    }
}