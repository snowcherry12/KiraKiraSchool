using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEngine;

namespace GameCreator.Editor.Inventory
{
    public class CoinTool : TPolymorphicItemTool
    {
        private static readonly IIcon ICON_COIN = new IconCoin(ColorTheme.Type.Yellow);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Coin-Head",
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Coin-Body",
        };

        public override string Title => this.m_Property.GetValue<Coin>()?.ToString();

        protected override Color Color => ColorTheme.Get(ColorTheme.Type.TextNormal);

        protected override object Value => null;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public CoinTool(IPolymorphicListTool parentTool, int index)
            : base(parentTool, index)
        { }

        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        protected override Texture2D GetIcon()
        {
            return ICON_COIN.Texture;
        }
    }
}