using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class ComboItem
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private bool m_IsDisabled;
        
        [SerializeField] private MeleeKey m_Key = MeleeKey.A;
        [SerializeField] private MeleeMode m_Mode = MeleeMode.Tap;
        [SerializeField] private MeleeExecute m_When = MeleeExecute.InOrder;
        
        [SerializeField] private bool m_AutoRelease;
        [SerializeField] private PropertyGetDecimal m_Timeout = GetDecimalConstantOne.Create;
        
        [SerializeField] private EnablerFloat m_HasDelay = new EnablerFloat(false, 0.5f);

        [SerializeField] private RunConditionsList m_Conditions = new RunConditionsList();
        
        [SerializeField] private Skill m_Skill;

        // PROPERTIES: ----------------------------------------------------------------------------

        public MeleeKey Key => this.m_Key;
        public MeleeMode Mode => this.m_Mode;
        public MeleeExecute When => this.m_When;

        public Skill Skill => this.m_Skill;

        public bool AutoRelease => this.m_AutoRelease;
        
        // GETTER METHODS: ------------------------------------------------------------------------

        public float GetTimeout(Args args) => (float) this.m_Timeout.Get(args);

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool CheckConsumeExecuteCharge(Input input, float chargeDuration, Args args)
        {
            if (this.m_IsDisabled) return false;
            if (this.Skill == null) return false;
            if (this.m_Key != input.ExecuteKey) return false;
            if (this.m_Mode == MeleeMode.Tap) return false;
            
            float timeout = this.GetTimeout(args);
            if (chargeDuration < timeout) return false;

            if (!this.m_HasDelay.IsEnabled) return true;
            return this.m_HasDelay.Value <= input.TimeBetweenExecutions;
        }
        
        public bool CheckConsumeExecuteTap(Input input, Args args)
        {
            if (this.m_IsDisabled) return false;
            if (this.Skill == null) return false;
            if (this.m_Key != input.ExecuteKey) return false;
            if (this.m_Mode == MeleeMode.Charge) return false;

            if (!this.m_HasDelay.IsEnabled) return true;
            return this.m_HasDelay.Value <= input.TimeBetweenExecutions;
        }
        
        public bool CheckConsumeCharge(Input input, Args args)
        {
            if (this.m_IsDisabled) return false;
            if (this.Skill == null) return false;
            if (this.Mode != MeleeMode.Charge) return false;

            if (input.ChargeActiveFrame != Time.frameCount) return false;
            if (this.m_Key != input.ChargeKey) return false;

            if (!this.m_HasDelay.IsEnabled) return true;
            return this.m_HasDelay.Value <= input.TimeBetweenExecutions;
        }
        
        public bool CheckConditions(Args args)
        {
            return this.m_Conditions.Check(args);
        }
        
        // EDITOR INTERNAL METHODS: ---------------------------------------------------------------

        internal void OnBeforeSerializeRoot()
        {
            if (!this.m_HasDelay.IsEnabled) return;
            
            float previousValue = this.m_HasDelay.Value;
            this.m_HasDelay = new EnablerFloat(false, previousValue);
        }

        // TO STRING: -----------------------------------------------------------------------------

        public override string ToString() => this.m_Skill != null 
            ? this.m_Skill.name 
            : "(none)";
    }
}