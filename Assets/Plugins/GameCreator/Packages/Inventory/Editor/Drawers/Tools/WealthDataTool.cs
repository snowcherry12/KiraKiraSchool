using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Inventory
{
    public class WealthDataTool : TPolymorphicItemTool
    {
        private static readonly IIcon ICON_STOCK = new IconCurrency(ColorTheme.Type.Yellow);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/WealthData-Head",
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/WealthData-Body",
        };

        public override string Title
        {
            get
            {
                SerializedProperty currency = this.m_Property
                    .FindPropertyRelative(WealthDataDrawer.PROP_CURRENCY);
                
                SerializedProperty amount = this.m_Property
                    .FindPropertyRelative(WealthDataDrawer.PROP_AMOUNT);

                return currency.objectReferenceValue != null 
                    ? $"{currency.objectReferenceValue.name} +{amount.intValue}" 
                    : "(none)";
            }
        }

        protected override Color Color => ColorTheme.Get(ColorTheme.Type.TextNormal);

        protected override object Value => null;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public WealthDataTool(IPolymorphicListTool parentTool, int index)
            : base(parentTool, index)
        { }

        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        protected override Texture2D GetIcon()
        {
            return ICON_STOCK.Texture;
        }
    }
}