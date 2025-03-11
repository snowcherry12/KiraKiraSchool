using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Shooter;
using UnityEngine;

namespace GameCreator.Editor.Shooter
{
    public class SightItemTool : TPolymorphicItemTool
    {
        private static readonly IIcon ICON_SIGHT = new IconSight(ColorTheme.Type.Red);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Shooter/Editor/StyleSheets/Sight-Head",
            EditorPaths.PACKAGES + "Shooter/Editor/StyleSheets/Sight-Body"
        };

        protected override object Value => null;

        public override string Title => this.Index == 0 && this.ParentTool.PropertyList.arraySize > 1
            ? $"{base.Title} (default)"
            : base.Title;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public SightItemTool(IPolymorphicListTool parentTool, int index)
            : base(parentTool, index)
        { }
        
        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        protected override Texture2D GetIcon()
        {
            return ICON_SIGHT.Texture;
        }
    }
}