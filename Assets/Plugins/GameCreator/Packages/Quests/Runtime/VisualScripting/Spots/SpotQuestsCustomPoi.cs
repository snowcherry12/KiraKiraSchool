using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests
{
    [Title("Point of Interest")]
    [Image(typeof(IconPointInterest), ColorTheme.Type.Blue)]
    
    [Category("Quests/Point of Interest")]
    [Description(
        "Determines the position of a Point of Interest in order to show it " +
        "when it's active on a minimap or compass"
    )]

    [Serializable]
    public class SpotQuestsCustomPoi : TSpotPoi
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetString m_Name = GetStringEmpty.Create;
        [SerializeField] private PropertyGetSprite m_Sprite = GetSpriteNone.Create;
        [SerializeField] private PropertyGetColor m_Color = GetColorColorsWhite.Create;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => "Point of Interest";

        public override string GetName => this.m_Name.Get(this.Args);
        public override Sprite GetSprite => this.m_Sprite.Get(this.Args);
        public override Color GetColor => this.m_Color.Get(this.Args);

        // CALLBACKS: -----------------------------------------------------------------------------

        protected override void OnHotspotActivate()
        {
            this.Refresh();
        }
        
        protected override void OnHotspotDeactivate()
        {
            this.Refresh();
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override void Refresh()
        {
            if (this.HasBeenDisabled)
            {
                PointsOfInterest.Remove(this.Id);
                return;
            }
            
            switch (this.Hotspot.IsActive && this.Hotspot.isActiveAndEnabled)
            {
                case true: PointsOfInterest.Insert(this.Id, this); break;
                case false: PointsOfInterest.Remove(this.Id); break;
            }
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override bool Filter(bool hiddenQuests, bool hideUntracked)
        {
            return true;
        }
    }
}
