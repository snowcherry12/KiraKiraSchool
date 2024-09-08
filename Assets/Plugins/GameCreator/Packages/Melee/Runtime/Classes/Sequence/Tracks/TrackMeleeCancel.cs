using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class TrackMeleeCancel : Track
    {
        private const ColorTheme.Type HANDLE_COLOR = ColorTheme.Type.Red;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeReference] private ClipMeleeCancel[] m_Clips = 
        {
            new ClipMeleeCancel()
        };

        // PROPERTIES: ----------------------------------------------------------------------------

        public override IClip[] Clips => this.m_Clips;
        
        public override TrackType TrackType => TrackType.Range;
        public override TrackAddType AllowAdd => TrackAddType.OnlyOne;
        public override TrackRemoveType AllowRemove => TrackRemoveType.Allow;
        
        public override Color ColorConnectionMiddleNormal => ColorTheme.Get(ColorTheme.Type.Red);

        public override bool IsConnectionLeftThin => false;
        public override bool IsConnectionMiddleThin => true;
        public override bool IsConnectionRightThin => false;

        public override Color ColorClipNormal => ColorTheme.Get(HANDLE_COLOR);
        public override Color ColorClipSelect => ColorTheme.Get(HANDLE_COLOR);
        
        public override Texture CustomClipIconNormal =>
            new IconMeleeSequenceClipCancel(this.ColorClipNormal).Texture;
        
        public override Texture CustomClipIconSelect =>
            new IconMeleeSequenceClipCancel(this.ColorClipSelect).Texture;

        public override bool HasInspector => false;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public TrackMeleeCancel()
        { }

        public TrackMeleeCancel(ClipMeleeCancel clip)
        {
            this.m_Clips = new[] { clip };
        }
    }
}