using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class TrackMeleeRootMotionRotation : Track
    {
        private const ColorTheme.Type HANDLE_COLOR = ColorTheme.Type.Teal;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeReference] private ClipMeleeRootMotionRotation[] m_Clips = 
        {
            new ClipMeleeRootMotionRotation()
        };

        // PROPERTIES: ----------------------------------------------------------------------------

        public override IClip[] Clips => this.m_Clips;
        
        public override TrackType TrackType => TrackType.Single;
        public override TrackAddType AllowAdd => TrackAddType.OnlyOne;
        public override TrackRemoveType AllowRemove => TrackRemoveType.Allow;

        public override Color ColorConnectionLeftNormal => ColorTheme.Get(HANDLE_COLOR);

        public override Color ColorClipNormal => ColorTheme.Get(HANDLE_COLOR);
        public override Color ColorClipSelect => ColorTheme.Get(HANDLE_COLOR);

        public override Texture CustomClipIconNormal =>
            new IconMeleeSequenceClipRotation(this.ColorClipNormal).Texture;

        public override Texture CustomClipIconSelect =>
            new IconMeleeSequenceClipRotation(this.ColorClipSelect).Texture;
        
        public override bool HasInspector => false;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public TrackMeleeRootMotionRotation()
        { }

        public TrackMeleeRootMotionRotation(ClipMeleeRootMotionRotation clip)
        {
            this.m_Clips = new[] { clip };
        }
    }
}