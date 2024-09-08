using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Inventory
{
    public class PropertyTool : TPolymorphicItemTool
    {
        private static readonly IIcon ICON_PROPERTY = new IconProperty(ColorTheme.Type.Yellow);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Property-Head",
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Property-Body",
        };

        public override string Title
        {
            get
            {
                SerializedProperty propertyID = this.m_Property
                    .FindPropertyRelative(ItemPropertyDrawer.PROP_PROPERTY_ID);
                
                string text = propertyID
                    .FindPropertyRelative(IdStringDrawer.NAME_STRING)
                    .stringValue;
                
                if (string.IsNullOrEmpty(text)) return "(none)";
                
                string num = ItemPropertyDrawer.GetNumberValue(this.m_Property);
                num = num != "0" ? $"({num}) " : string.Empty;
                
                return $"{text} {num}";
            }
        }

        protected override Color Color
        {
            get
            {
                bool isHidden = this.m_Property
                    .FindPropertyRelative(ItemPropertyDrawer.PROP_VISIBLE)
                    .enumValueIndex == (int) Property.Visibility.Hidden;
                
                return isHidden 
                    ? ColorTheme.Get(ColorTheme.Type.TextLight)
                    : ColorTheme.Get(ColorTheme.Type.TextNormal);
            }
        }

        protected override object Value => null;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public PropertyTool(IPolymorphicListTool parentTool, int index)
            : base(parentTool, index)
        { }

        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        protected override Texture2D GetIcon()
        {
            return ICON_PROPERTY.Texture;
        }
    }
}