using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Inventory
{
    public class EquipmentItemTool : TPolymorphicItemTool
    {
        private static readonly IIcon ICON_EQUIPMENT_SLOT = new IconEquipment(ColorTheme.Type.Blue);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Equipment-Head",
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Equipment-Body",
        };

        public override string Title
        {
            get
            {
                SerializedProperty propertyValue = this.m_Property
                    .FindPropertyRelative(EquipmentSlotDrawer.PROP_BASE);
                
                string text = propertyValue.objectReferenceValue != null
                    ? propertyValue.objectReferenceValue.name
                    : "(none)";

                return TextUtils.Humanize(text);
            }
        }

        protected override Color Color => ColorTheme.Get(ColorTheme.Type.TextNormal);

        protected override object Value => null;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public EquipmentItemTool(IPolymorphicListTool parentTool, int index)
            : base(parentTool, index)
        { }

        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        protected override Texture2D GetIcon()
        {
            return ICON_EQUIPMENT_SLOT.Texture;
        }
    }
}