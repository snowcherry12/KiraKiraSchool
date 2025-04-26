using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Title("On Actor Finish Line")]
    [Category("Dialogue/On Actor Finish Line")]
    [Description("Executed when the Actor referencing this Trigger stops playing a new line")]

    [Image(typeof(IconExpression), ColorTheme.Type.Green, typeof(OverlayCross))]
    
    [Keywords("Node", "Conversation", "Speech", "Text")]
    [Keywords("Play", "New", "Next", "Continue", "Skip")]

    [Serializable]
    public class EventDialogueActorFinishLine : VisualScripting.Event
    {
        protected override void OnReceiveCommand(Trigger trigger, CommandArgs args)
        {
            base.OnReceiveCommand(trigger, args);
            if (args.Command != Actor.COMMAND_ACTOR_FINISH) return;
            
            _ = trigger.Execute(args.Target);
        }
    }
}