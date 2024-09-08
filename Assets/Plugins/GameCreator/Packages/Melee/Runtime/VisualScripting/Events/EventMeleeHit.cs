using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Title("On Melee Hit")]
    [Category("Melee/On Melee Hit")]
    [Description("Executed when the Trigger receives a hit from a melee Skill")]

    [Image(typeof(IconReaction), ColorTheme.Type.Red)]
    
    [Keywords("Active", "Disable", "Inactive")]

    [Serializable]
    public class EventMeleeHit : VisualScripting.Event
    {
        public static readonly PropertyName COMMAND_HIT = "on-melee-hit";

        public override bool RequiresCollider => true;

        // METHODS: -------------------------------------------------------------------------------
        
        protected override void OnReceiveCommand(Trigger trigger, CommandArgs args)
        {
            base.OnReceiveCommand(trigger, args);
            
            if (args.Command != COMMAND_HIT) return;
            _ = trigger.Execute(args.Target);
        }
    }
}