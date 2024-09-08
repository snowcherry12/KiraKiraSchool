using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Melee
{
    public class AttackCharge : TAttackState
    {
        private const float DELAY = 0.15f;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private bool m_HasState;
        [NonSerialized] private float m_StartTime;

        [NonSerialized] private bool m_AutoRelease;
        [NonSerialized] private float m_Timeout;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override MeleePhase Phase => MeleePhase.Charge;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public AttackCharge(Attacks attacks) : base(attacks)
        { }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override bool TryToCancel()
        {
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
            
            Character self = this.Attacks.MeleeStance.Character;
            
            this.m_StartTime = self.Time.Time;
            
            ComboItem charge = this.Attacks.Combos?.Get(this.Attacks.ChargeId);
            
            this.m_AutoRelease = charge is { AutoRelease: true };
            this.m_Timeout = charge?.GetTimeout(this.Attacks.MeleeStance.Args) ?? 0f;

            self.Combat.Block.LowerGuard();
            self.Busy.SetBusy();

            this.m_HasState = this.ChargeSkill.ChargeState.IsValid(self);

            if (this.m_HasState)
            {
                ConfigState state = new ConfigState(
                    DELAY, 1f, 1f, 
                    this.ChargeSkill.TransitionIn, 
                    this.ChargeSkill.TransitionOut
                );
            
                _ = self.States.SetState(
                    this.ChargeSkill.ChargeState, 
                    this.ChargeSkill.ChargeLayer,
                    BlendMode.Blend, state
                );   
            }

            this.UpdateInput();
        }

        protected override void WhenExit(IStateMachine stateMachine)
        {
            base.WhenExit(stateMachine);

            Character self = this.Attacks.MeleeStance.Character;
            self.Busy.SetAvailable();
            
            Skill nextSkill = this.Attacks.Combos?.Get(this.Attacks.ComboId)?.Skill;
            
            bool isAvailable = self.States.IsAvailable(this.ChargeSkill.ChargeLayer);
            if (isAvailable) return;
            
            bool hasNextAttack = nextSkill != null;

            float delay = hasNextAttack ? nextSkill.TransitionIn : Attacks.MIN_CHARGE_DURATION;
            float transition = hasNextAttack ? 0f : this.ChargeSkill.TransitionOut;
            
            if (this.m_HasState)
            {
                self.States.Stop(
                    this.ChargeSkill.ChargeLayer, 
                    delay, transition
                );
            }
        }
        
        // UPDATE METHOD: -------------------------------------------------------------------------

        protected override void WhenUpdate(IStateMachine stateMachine)
        {
            base.WhenUpdate(stateMachine);
            
            bool hasSuccessInput = this.UpdateInput();
            if (!hasSuccessInput) this.Attacks.ToNone();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private bool UpdateInput()
        {
            if (this.m_AutoRelease)
            {
                float time = this.Attacks.MeleeStance.Character.Time.Time;
                if (time - this.m_StartTime >= this.m_Timeout)
                {
                    MeleeKey currentChargeKey = this.Attacks.Input.ChargeKey;
                    this.Attacks.Input.InputExecute(currentChargeKey);
                }
            }

            if (this.Attacks.Input.HasChargeInQueue)
            {
                int chargeId = this.Attacks.ChargeId;
                return this.UpdateCharge(chargeId);
            }
            
            if (this.Attacks.Input.HasExecuteInQueue)
            {
                int chargeId = this.Attacks.ChargeId;
                int comboId = this.Attacks.ComboId;
                
                return this.UpdateExecute(chargeId, comboId);
            }

            return true;
        }
    }
}