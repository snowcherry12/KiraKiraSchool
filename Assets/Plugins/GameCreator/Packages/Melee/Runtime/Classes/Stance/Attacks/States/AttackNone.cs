using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    public class AttackNone : TAttackState
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override MeleePhase Phase => MeleePhase.None;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public AttackNone(Attacks attacks) : base(attacks)
        { }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override bool TryToCancel()
        {
            return true;
        }
        
        public override void ForceCancel()
        { }

        protected override void WhenEnter(IStateMachine stateMachine)
        {
            base.WhenEnter(stateMachine);
            this.UpdateInput();
        }

        // TRANSITION METHOD: ---------------------------------------------------------------------

        protected override void WhenUpdate(IStateMachine stateMachine)
        {
            base.WhenUpdate(stateMachine);
            this.UpdateInput();
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
    }
}