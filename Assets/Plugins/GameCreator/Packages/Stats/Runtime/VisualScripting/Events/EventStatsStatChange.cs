using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("On Stat Change")]
    [Category("Stats/On Stat Change")]
    [Description("Executed when the value of a specific game object's Stat is modified. Including due to Stat Modifiers")]

    [Image(typeof(IconStat), ColorTheme.Type.Red)]

    [Parameter("Target", "The targeted game object with a Traits component")]
    [Parameter("When", "Determines if the event executes when the Stat increases, decreases or both")]
    [Parameter("Stat", "The Stat from which the event detects its changes")]
    
    [Keywords("Health", "HP", "Mana", "MP", "Stamina")]

    [Serializable]
    public class EventStatsStatChange : VisualScripting.Event
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

        [SerializeField] private PropertyGetStat m_Stat = new PropertyGetStat();

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Traits m_TargetTraits;
        [NonSerialized] private Stat m_TargetStat;
        
        [NonSerialized] private double m_LastValue;

        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);

            this.m_TargetStat = this.m_Stat.Get(trigger.gameObject);
            if (this.m_TargetStat == null) return;

            this.m_TargetTraits = this.m_Target.Get<Traits>(trigger.gameObject);
            if (this.m_TargetTraits == null) return;
            
            this.m_TargetTraits.EventChange += this.OnChange;
            this.m_LastValue = this.m_TargetTraits.RuntimeStats.Get(this.m_TargetStat.ID).Value;
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
            if (this.m_Stat == null) return;
            
            double nextValue = this.m_TargetTraits.RuntimeStats.Get(this.m_TargetStat.ID).Value;
            double prevValue = this.m_LastValue;
            
            this.m_LastValue = nextValue;
            
            if (Math.Abs(nextValue - prevValue) < float.Epsilon) return;
            if (this.m_When == DetectionType.OnIncrease && nextValue <= prevValue) return;
            if (this.m_When == DetectionType.OnDecrease && nextValue >= prevValue) return;
            
            this.m_Trigger.Execute(this.m_TargetTraits.gameObject);
        }
    }
}