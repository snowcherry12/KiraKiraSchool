using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("On Interact")]
    [Image(typeof(IconCharacterInteract), ColorTheme.Type.Green)]
    
    [Category("Interactive/On Interact")]
    [Description("Executed when a Character interacts with this Trigger")]
    
    [Parameter("Use Raycast", "Checks if there is something between the character and the Trigger")]
    [Example("The 'Use Raycast' option checks if there is no other collider between the Character and the Trigger")]

    [Serializable]
    public class EventCharacterOnInteract : Event
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private CompareGameObjectOrAny m_FromCharacter = new CompareGameObjectOrAny();
        [SerializeField] private UseRaycast m_UseRaycast = new UseRaycast();

        // INTERACTION: ---------------------------------------------------------------------------

        protected internal override bool OnInteract(Trigger trigger, Character character)
        {
            base.OnInteract(trigger, character);

            if (!this.IsActive) return false;
            if (character == null) return false;

            if (!this.m_FromCharacter.Match(character.gameObject, trigger.gameObject)) return false;
            if (this.m_UseRaycast.HasObstacle(character.transform, trigger.transform)) return false;
            
            _ = this.m_Trigger.Execute(character.gameObject);
            return true;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        protected internal override void OnAwake(Trigger trigger)
        {
            base.OnAwake(trigger);
            trigger.RequireInteractionTracker();
        }
    }
}