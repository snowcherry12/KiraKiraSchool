using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("On Attribute Change")]
    [Category("Stats/On Attribute Change")]
    [Description("Executed when the value of a specific game object's Attribute is modified")]

    [Image(typeof(IconAttr), ColorTheme.Type.Blue)]

    [Parameter("Target", "The targeted game object with a Traits component")]
    [Parameter("When", "Determines if the event executes when the Attribute increases, decreases or both")]
    [Parameter("Attribute", "The Attribute from which the event detects its changes")]
    
    [Keywords("Health", "HP", "Mana", "MP", "Stamina")]

    [Serializable]
    public class EventStatsAttributeChange : VisualScripting.Event
    {
        private enum DetectionType
        {
            OnChange,
            OnIncrease,
            OnDecrease
        }

        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();

        [SerializeField] private DetectionType m_When = DetectionType.OnChange;

        [SerializeField] private PropertyGetAttribute m_Attribute = new PropertyGetAttribute();

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Traits m_TargetTraits;
        [NonSerialized] private Attribute m_TargetAttribute;
        
        [NonSerialized] private double m_LastValue;

        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);

            this.m_TargetAttribute = this.m_Attribute.Get(trigger.gameObject);
            if (this.m_TargetAttribute == null) return;

            this.m_TargetTraits = this.m_Target.Get<Traits>(trigger.gameObject);
            if (this.m_TargetTraits == null) return;
            
            this.m_TargetTraits.EventChange += this.OnChange;
            this.m_LastValue = this.m_TargetTraits.RuntimeAttributes.Get(m_TargetAttribute.ID).Value;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            
            if (this.m_TargetTraits == null) return;
            this.m_TargetTraits.EventChange -= this.OnChange;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnChange()
        {
            if (this.m_TargetAttribute == null) return;

            double nextLValue = this.m_TargetTraits
                .RuntimeAttributes
                .Get(this.m_TargetAttribute.ID).Value;
            
            double prevValue = this.m_LastValue;
            this.m_LastValue = nextLValue;

            if (Math.Abs(nextLValue - prevValue) < float.Epsilon) return;
            if (this.m_When == DetectionType.OnIncrease && nextLValue <= prevValue) return;
            if (this.m_When == DetectionType.OnDecrease && nextLValue >= prevValue) return;
            
            _ = this.m_Trigger.Execute(this.m_TargetTraits.gameObject);
        }
    }
}