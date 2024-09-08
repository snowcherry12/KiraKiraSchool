using System;

namespace GameCreator.Runtime.Melee
{
    public class Input
    {
        private const float BUFFER_WINDOW = 0.5f;

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private readonly MeleeStance m_MeleeStance;

        [NonSerialized] private bool m_IsCharging;
        [NonSerialized] private readonly InputControl m_InputCharge;
        [NonSerialized] private readonly InputControl m_InputExecute;

        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public float BufferWindow { get; set; } = BUFFER_WINDOW;

        public bool HasChargeInQueue => this.m_InputCharge.HasInQueue;
        public bool HasExecuteInQueue => this.m_InputExecute.HasInQueue;

        public MeleeKey ChargeKey => this.m_InputCharge.ConsumeKey;
        public MeleeKey ExecuteKey => this.m_InputExecute.ConsumeKey;
        
        public int ChargeActiveFrame => this.m_InputCharge.ActiveFrame;
        public int ExecuteActiveFrame => this.m_InputExecute.ActiveFrame;

        public float TimeBetweenExecutions => this.m_InputExecute.ElapseBetweenInputs;
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action<MeleeKey> EventInputCharge;
        public event Action<MeleeKey> EventInputExecute;
        public event Action<MeleeKey> EventUseCharge;
        public event Action<MeleeKey> EventUseExecute;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Input(MeleeStance meleeStance)
        {
            this.m_MeleeStance = meleeStance;

            this.m_InputCharge = new InputControl(this, this.m_MeleeStance);
            this.m_InputExecute = new InputControl(this, this.m_MeleeStance);
            
            this.m_IsCharging = false;
        }
        
        // CANCEL METHODS: ------------------------------------------------------------------------
        
        public void Cancel()
        {
            this.m_InputCharge.Cancel();
            this.m_InputExecute.Cancel();
        }
        
        // INPUT METHODS: -------------------------------------------------------------------------

        internal void InputCharge(MeleeKey key)
        {
            this.m_InputCharge.Input(key);
            this.EventInputCharge?.Invoke(key);
        }

        internal void InputExecute(MeleeKey key)
        {
            this.m_InputExecute.Input(key);
            this.EventInputExecute?.Invoke(key);
        }
        
        // CONSUME METHODS: -----------------------------------------------------------------------
        
        internal bool ConsumeCharge()
        {
            this.m_IsCharging = this.m_InputCharge.Consume();
            if (this.m_IsCharging)
            {
                this.EventUseCharge?.Invoke(this.ChargeKey);
                return true;
            }

            return false;
        }

        internal bool ConsumeExecute(out float chargeDuration)
        {
            bool success = this.m_InputExecute.Consume();
            chargeDuration = success 
                ? this.m_MeleeStance.Character.Time.Time - this.m_InputCharge.ConsumeTime
                : -1f;
            
            if (success) this.EventUseExecute?.Invoke(this.ExecuteKey);
            return success;
        }
    }
}