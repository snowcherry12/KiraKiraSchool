using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Shooter
{
    [Serializable]
    public class TrackReloadMagazine : Track
    {
        private const ColorTheme.Type HANDLE_COLOR = ColorTheme.Type.Yellow;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeReference] private ClipReloadMagazine[] m_Clips = 
        {
            new ClipReloadMagazine()
        };

        // PROPERTIES: ----------------------------------------------------------------------------

        public override IClip[] Clips => this.m_Clips;
        
        public override TrackType TrackType => TrackType.Range;
        public override TrackAddType AllowAdd => TrackAddType.OnlyOne;
        public override TrackRemoveType AllowRemove => TrackRemoveType.Allow;
        
        public override Color ColorConnectionMiddleNormal => ColorTheme.Get(HANDLE_COLOR);

        public override bool IsConnectionLeftThin => false;
        public override bool IsConnectionMiddleThin => false;
        public override bool IsConnectionRightThin => false;

        public override Texture CustomClipIconNormal =>
            new IconShooterSequenceClipMagazine(this.ColorClipNormal).Texture;
        
        public override Texture CustomClipIconSelect =>
            new IconShooterSequenceClipMagazine(this.ColorClipSelect).Texture;
        
        public override bool HasInspector => true;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public TrackReloadMagazine()
        { }

        public TrackReloadMagazine(ClipReloadMagazine clip)
        {
            this.m_Clips = new[] { clip };
        }
    }
}