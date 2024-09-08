using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [Title("On Status Effect Change")]
    [Category("Stats/On Status Effect Change")]
    [Description("Executed when a Status Effect is added or removed from a Traits component")]

    [Image(typeof(IconStatusEffect), ColorTheme.Type.Green)]

    [Parameter("Target", "The targeted game object with a Traits component")]
    [Parameter("Status Effect", "Determines if the event detects any Status Effect change or a specific one")]

    [Keywords("Buff", "Debuff", "Enhance", "Ailment")]
    [Keywords(
        "Blind", "Dark", "Burn", "Confuse", "Dizzy", "Stagger", "Fear", "Freeze", "Paralyze", 
        "Shock", "Silence", "Sleep", "Silence", "Slow", "Toad", "Weak", "Strong", "Poison"
    )]
    [Keywords(
        "Haste", "Protect", "Reflect", "Regenerate", "Shell", "Armor", "Shield", "Berserk",
        "Focus", "Raise"
    )]

    [Serializable]
    public class EventStatsStatusEffectChange : VisualScripting.Event
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();
        [SerializeField] private StatusEffectOrAny m_StatusEffect = new StatusEffectOrAny();

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Traits m_TargetTraits;
        [NonSerialized] private Args m_Args;

        // INITIALIZERS: --------------------------------------------------------------------------
        
        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);

            this.m_Args = new Args(trigger.gameObject);
            
            if (this.m_TargetTraits != null)
            {
                this.m_TargetTraits.RuntimeStatusEffects.EventChange -= this.OnChange;
            }
            
            this.m_TargetTraits = this.m_Target.Get<Traits>(this.m_Args);
            if (this.m_TargetTraits == null) return;

            this.m_TargetTraits.RuntimeStatusEffects.EventChange += this.OnChange;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            if (this.m_TargetTraits == null) return;

            this.m_TargetTraits.RuntimeStatusEffects.EventChange -= this.OnChange;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnChange(IdString sourceStatusEffectID)
        {
            if (this.m_StatusEffect.Match(sourceStatusEffectID, this.m_Args))
            {
                _ = this.m_Trigger.Execute(this.m_TargetTraits.gameObject);
            }
        }
    }
}