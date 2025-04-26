using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("On Actor Start Line")]
    [Category("Dialogue/On Actor Start Line")]
    [Description("Executed when the Actor referencing this Trigger starts playing a new line")]

    [Image(typeof(IconExpression), ColorTheme.Type.Green, typeof(OverlayArrowRight))]
    
    [Keywords("Node", "Conversation", "Speech", "Text")]
    [Keywords("Play", "New", "Next", "Continue", "Skip")]

    [Serializable]
    public class EventDialogueActorStartsLine : VisualScripting.Event
    {
        protected override void OnReceiveCommand(Trigger trigger, CommandArgs args)
        {
            base.OnReceiveCommand(trigger, args);
            if (args.Command != Actor.COMMAND_ACTOR_START) return;
            
            _ = trigger.Execute(args.Target);
        }
    }
}