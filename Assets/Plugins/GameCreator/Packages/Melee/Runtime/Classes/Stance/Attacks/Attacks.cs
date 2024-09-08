using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    public class Attacks : TStateMachine
    {
        public const float MIN_CHARGE_DURATION = 0.2f;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly AttackNone m_None;
        [NonSerialized] private readonly AttackSkill m_Skill;
        [NonSerialized] private readonly AttackCharge m_Charge;
        [NonSerialized] private readonly AttackReaction m_Reaction;

        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public MeleeStance MeleeStance { get; }

        public MeleePhase Phase => (this.Current as TAttackState)?.Phase ?? MeleePhase.None;
        
        [field: NonSerialized] public Input Input { get; }

        [field: NonSerialized] public MeleeWeapon Weapon { get; private set; }
        [field: NonSerialized] public ComboTree Combos { get; private set; }
        
        [field: NonSerialized] public int ChargeId  { get; private set; }
        [field: NonSerialized] public int ComboId  { get; private set; }
        
        [field: NonSerialized] public Skill ChargeSkill  { get; private set; }
        [field: NonSerialized] public Skill ComboSkill  { get; private set; }

        [field: NonSerialized] public IReaction ReactionAsset { get; private set; }
        [field: NonSerialized] public ReactionInput ReactionInput { get; private set; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public Attacks(MeleeStance meleeStance, Input input)
        {
            this.MeleeStance = meleeStance;

            this.m_None = new AttackNone(this);
            this.m_Skill = new AttackSkill(this);
            this.m_Charge = new AttackCharge(this);
            this.m_Reaction = new AttackReaction(this);

            this.Combos = null;
            this.ComboId = ComboTree.NODE_INVALID;
            
            this.ReactionAsset = null;
            this.ReactionInput = new ReactionInput();

            this.Input = input;
        }
        
        // UPDATE METHODS: ------------------------------------------------------------------------

        public void Update()
        {
            this.OnUpdate();
        }
        
        // TRANSITION METHODS: --------------------------------------------------------------------

        public void ToNone()
        {
            this.Weapon = null;
            this.Combos = null;
            
            this.ChargeId = ComboTree.NODE_INVALID;
            this.ComboId = ComboTree.NODE_INVALID;
            
            this.MeleeStance.Args.ChangeTarget(this.MeleeStance.Character.Combat.Targets.Primary);
            
            this.Weapon = null;
            
            this.ChargeSkill = null;
            this.ComboSkill = null;
            
            this.Change(this.m_None);
        }
        
        public void ToCharge(MeleeWeapon weapon, ComboTree combos, ChargeMatch match)
        {
            this.Weapon = weapon;
            this.Combos = combos;

            this.ChargeId = match.ChargeId;
            this.ComboId = match.PreviousComboId;
            
            this.MeleeStance.Args.ChangeTarget(this.MeleeStance.Character.Combat.Targets.Primary);
            
            this.ChargeSkill = this.Combos?.Get(this.ChargeId)?.Skill;
            this.ComboSkill = this.Combos?.Get(this.ComboId)?.Skill;
            
            this.Change(this.m_Charge);
        }

        public void ToSkill(MeleeWeapon weapon, ComboTree combos, int comboId)
        {
            this.Weapon = weapon;
            this.Combos = combos;

            this.ChargeId = ComboTree.NODE_INVALID;
            this.ComboId = comboId;

            this.MeleeStance.Args.ChangeTarget(this.MeleeStance.Character.Combat.Targets.Primary);

            this.ChargeSkill = null;
            this.ComboSkill = this.Combos?.Get(this.ComboId)?.Skill;
            
            this.Change(this.m_Skill);
        }
        
        public void ToSkill(MeleeWeapon weapon, Skill skill, GameObject target)
        {
            this.Weapon = weapon;
            this.Combos = null;
            
            this.ChargeId = ComboTree.NODE_INVALID;
            this.ComboId = ComboTree.NODE_INVALID;
            
            this.MeleeStance.Args.ChangeTarget(target != null 
                ? target
                : this.MeleeStance.Character.Combat.Targets.Primary
            );

            this.ChargeSkill = null;
            this.ComboSkill = skill;
            
            this.Change(this.m_Skill);
        }

        public void ToReact(GameObject target, IReaction reaction, ReactionInput input)
        {
            this.Weapon = null;
            this.Combos = null;
            
            this.ChargeId = ComboTree.NODE_INVALID;
            this.ComboId = ComboTree.NODE_INVALID;
            
            this.MeleeStance.Args.ChangeTarget(target);

            this.ChargeSkill = null;
            this.ComboSkill = null;
            
            this.ReactionAsset = reaction;
            this.ReactionInput = input;

            this.Change(this.m_Reaction);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool TryToCancel()
        {
            return (this.Current as TAttackState)?.TryToCancel() ?? true;
        }

        public void ForceCancel()
        {
            (this.Current as TAttackState)?.ForceCancel();
        }

        public void ResetHitsBuffer()
        {
            if (this.Current is AttackSkill attackSkill)
            {
                attackSkill.ClearHits();
            }
        }
    }
}