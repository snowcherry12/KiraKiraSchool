using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class TrackMeleePhases : Track
    {
        [SerializeReference] private ClipMeleePhases[] m_Clips = 
        {
            new ClipMeleePhases()
        };

        // PROPERTIES: ----------------------------------------------------------------------------

        public override IClip[] Clips => this.m_Clips;
        
        public override TrackType TrackType => TrackType.Range;
        public override TrackAddType AllowAdd => TrackAddType.OnlyOne;
        public override TrackRemoveType AllowRemove => TrackRemoveType.Allow;

        public override Color ColorConnectionLeftNormal => ColorTheme.Get(ColorTheme.Type.Blue);
        public override Color ColorConnectionMiddleNormal => ColorTheme.Get(ColorTheme.Type.Green);
        public override Color ColorConnectionRightNormal => ColorTheme.Get(ColorTheme.Type.Yellow);

        public override bool IsConnectionLeftThin => true;
        public override bool IsConnectionRightThin => true;

        public override Color ColorClipNormal => ColorTheme.Get(ColorTheme.Type.White);
        public override Color ColorClipSelect => ColorTheme.Get(ColorTheme.Type.White);

        public override bool HasInspector => false;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public TrackMeleePhases()
        {
            
        }

        public TrackMeleePhases(ClipMeleePhases clip)
        {
            this.m_Clips = new[] { clip };
        }
    }
}