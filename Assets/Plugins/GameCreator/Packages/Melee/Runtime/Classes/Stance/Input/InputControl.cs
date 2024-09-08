using System;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    internal class InputControl
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly MeleeStance m_MeleeStance;
        [NonSerialized] private readonly Input m_Input;

        [NonSerialized] private readonly Key m_InputKey = new Key();
        [NonSerialized] private readonly Key m_ConsumeKey = new Key();
        
        [NonSerialized] private float m_InputTime  = -1f;
        [NonSerialized] private float m_ConsumeTime = -1f;

        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] public int ActiveFrame { get; private set; }
        [field: NonSerialized] public float ElapseBetweenInputs { get; private set; }
        
        public bool HasInQueue
        {
            get
            {
                if (!this.m_InputKey.HasKey) return false;

                float currentTime = this.m_MeleeStance.Character.Time.Time;
                return currentTime - this.m_InputTime <= this.m_Input.BufferWindow;
            }
        }

        public float ConsumeTime => this.m_ConsumeTime >= 0f ? this.m_ConsumeTime : -1f;
        
        public MeleeKey ConsumeKey => this.m_ConsumeKey.Value;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public InputControl(Input input, MeleeStance meleeStance)
        {
            this.m_MeleeStance = meleeStance;
            this.m_Input = input;

            this.ActiveFrame = -1;
            this.ElapseBetweenInputs = -1f;
        }
        
        // CANCEL METHODS: ------------------------------------------------------------------------
        
        public void Cancel()
        {
            this.m_InputKey.Unset();
            this.m_ConsumeKey.Unset();
            
            this.m_InputTime = -1f;
            this.m_ConsumeTime = -1f;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Input(MeleeKey key)
        {
            this.m_InputKey.Set(key);

            float previousInputTime = this.m_InputTime;
            
            this.m_InputTime = this.m_MeleeStance.Character.Time.Time;
            this.ElapseBetweenInputs = this.m_InputTime - previousInputTime;
        }

        public bool Consume()
        {
            if (!this.m_InputKey.HasKey) return false;
            
            MeleeKey key = this.m_InputKey.Value;

            this.m_InputKey.Unset();
            this.m_ConsumeKey.Set(key);
            
            this.m_ConsumeTime = this.m_MeleeStance.Character.Time.Time;
            this.ActiveFrame = Time.frameCount;

            return true;
        }
    }
}