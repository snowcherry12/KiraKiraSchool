using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class TrackMeleeMotionWarping : Track
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeReference] private ClipMeleeMotionWarping[] m_Clips = 
        {
            new ClipMeleeMotionWarping()
        };

        // PROPERTIES: ----------------------------------------------------------------------------

        public override IClip[] Clips => this.m_Clips;
        
        public override TrackType TrackType => TrackType.Range;
        public override TrackAddType AllowAdd => TrackAddType.OnlyOne;
        public override TrackRemoveType AllowRemove => TrackRemoveType.Allow;

        public override Color ColorConnectionMiddleNormal => ColorTheme.Get(ColorTheme.Type.Purple);
        public override Color ColorConnectionMiddleSelect => ColorTheme.GetLighter(ColorTheme.Type.Purple);

        public override Color ColorClipNormal => ColorTheme.Get(ColorTheme.Type.White);
        public override Color ColorClipSelect => ColorTheme.Get(ColorTheme.Type.White);

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public TrackMeleeMotionWarping()
        { }

        public TrackMeleeMotionWarping(ClipMeleeMotionWarping clip)
        {
            this.m_Clips = new[] { clip };
        }
    }
}