using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using Event = GameCreator.Runtime.VisualScripting.Event;

namespace GameCreator.Runtime.Shooter
{
    [Title("On Shoot Hit")]
    [Category("Shooter/On Shoot Hit")]
    [Description("Executed when a shot hits the collider on this Trigger")]

    [Image(typeof(IconReaction), ColorTheme.Type.Yellow)]
    [Keywords("Fire", "Critical", "Gun", "Shot", "Impact")]

    [Serializable]
    public class EventShooterHit : Event
    {
        public static readonly PropertyName COMMAND_HIT = "on-shooter-hit";
        
        public override bool RequiresCollider => true;

        protected override void OnReceiveCommand(Trigger trigger, CommandArgs args)
        {
            base.OnReceiveCommand(trigger, args);
            if (args.Command != COMMAND_HIT) return;
            
            _ = trigger.Execute(args.Target);
        }
    }
}