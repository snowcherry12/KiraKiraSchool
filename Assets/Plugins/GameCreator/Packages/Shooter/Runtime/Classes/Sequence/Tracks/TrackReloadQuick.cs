using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class TrackReloadQuick : Track
    {
        private const ColorTheme.Type HANDLE_COLOR = ColorTheme.Type.Green;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeReference] private ClipReloadQuick[] m_Clips = Array.Empty<ClipReloadQuick>();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override IClip[] Clips => this.m_Clips;
        
        public override TrackType TrackType => TrackType.Range;
        public override TrackAddType AllowAdd => TrackAddType.OnlyOne;
        public override TrackRemoveType AllowRemove => TrackRemoveType.Allow;
        
        public override Color ColorConnectionMiddleNormal => ColorTheme.Get(HANDLE_COLOR);
        
        public override bool IsConnectionLeftThin => true;
        public override bool IsConnectionMiddleThin => false;
        public override bool IsConnectionRightThin => true;

        public override Color ColorClipNormal => ColorTheme.Get(ColorTheme.Type.Green);
        public override Color ColorClipSelect => ColorTheme.Get(ColorTheme.Type.Green);
        
        public override Texture CustomClipIconNormal =>
            new IconShooterSequenceClipCancel(this.ColorClipNormal).Texture;
        
        public override Texture CustomClipIconSelect =>
            new IconShooterSequenceClipCancel(this.ColorClipSelect).Texture;

        public override bool HasInspector => false;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public TrackReloadQuick()
        { }

        public TrackReloadQuick(ClipReloadQuick clip)
        {
            this.m_Clips = new[] { clip };
        }
    }
}