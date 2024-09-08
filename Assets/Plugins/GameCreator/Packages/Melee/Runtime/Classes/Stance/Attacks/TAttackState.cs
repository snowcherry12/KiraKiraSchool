using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    public abstract class TAttackState : TState
    {
        protected const int GRAVITY_INFLUENCE_KEY = 1;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public abstract MeleePhase Phase { get; }
        
        [field: NonSerialized] protected Attacks Attacks { get; }
        
        [field: NonSerialized] protected Skill ChargeSkill { get; private set; }
        [field: NonSerialized] protected Skill ComboSkill { get; private set; }

        protected float CurrentTime => this.Attacks.MeleeStance.Character.Time.Time;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TAttackState(Attacks attacks)
        {
            this.Attacks = attacks;
        }

        // ABSTRACT METHODS: ----------------------------------------------------------------------

        public abstract bool TryToCancel();
        public abstract void ForceCancel();
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------

        protected override void WhenEnter(IStateMachine stateMachine)
        {
            base.WhenEnter(stateMachine);
            
            this.ChargeSkill = this.Attacks.ChargeSkill;
            this.ComboSkill = this.Attacks.ComboSkill;
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected bool UpdateCharge(int previousComboId)
        {
            bool success = this.Attacks.Input.ConsumeCharge();
            if (!success) return false;
            
            foreach (Weapon weapon in this.Attacks.MeleeStance.Character.Combat.Weapons)
            {
                if (weapon.Asset is not MeleeWeapon meleeWeapon) continue;
                if (meleeWeapon.Combo == null) continue;
                
                ChargeMatch match = meleeWeapon.Combo.MatchCharge(
                    previousComboId,
                    this.Attacks.Input,
                    this.Attacks.MeleeStance.Args
                );
                
                if (match.ChargeId == ComboTree.NODE_INVALID) continue;
        
                ComboItem combo = meleeWeapon.Combo.Get(match.ChargeId);
                if (combo == null || combo.Skill == null) continue;
                
                this.Attacks.ToCharge(meleeWeapon, meleeWeapon.Combo, match);
                return true;
            }

            return false;
        }
        
        protected bool UpdateExecute(int previousChargeId, int previousComboId)
        {
            bool success = this.Attacks.Input.ConsumeExecute(out float chargeDuration);
            if (!success) return false;
            
            int chargeComboId = ComboTree.NODE_INVALID;

            if (this.Attacks.Combos != null)
            {
                chargeComboId = this.Attacks.Combos.MatchExecuteCharge(
                    previousChargeId,
                    this.Attacks.Input, 
                    chargeDuration,
                    this.Attacks.MeleeStance.Args
                );
            }

            if (chargeComboId != ComboTree.NODE_INVALID)
            {
                ComboItem combo = this.Attacks.Combos.Get(chargeComboId);
                if (combo != null && combo.Skill != null)
                {
                    this.Attacks.ToSkill(this.Attacks.Weapon, this.Attacks.Combos, chargeComboId);
                    return true;
                }
            }

            foreach (Weapon weapon in this.Attacks.MeleeStance.Character.Combat.Weapons)
            {
                if (weapon.Asset is not MeleeWeapon meleeWeapon) continue;
                if (meleeWeapon.Combo == null) continue;
                
                int comboId = meleeWeapon.Combo.MatchExecuteTap(
                    previousComboId,
                    this.Attacks.Input, 
                    this.Attacks.MeleeStance.Args
                );
                
                if (comboId == ComboTree.NODE_INVALID) continue;
        
                ComboItem combo = meleeWeapon.Combo.Get(comboId);
                if (combo == null || combo.Skill == null) continue;

                this.Attacks.ToSkill(meleeWeapon, meleeWeapon.Combo, comboId);
                return true;
            }

            this.Attacks.ToNone();
            return true;
        }
    }
}