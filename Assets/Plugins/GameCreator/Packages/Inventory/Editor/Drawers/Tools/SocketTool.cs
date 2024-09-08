using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine;

namespace GameCreator.Editor.Inventory
{
    public class SocketTool : TPolymorphicItemTool
    {
        private static readonly IIcon ICON_SOCKET = new IconSocket(ColorTheme.Type.Green);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Socket-Head",
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Socket-Body",
        };

        public override string Title
        {
            get
            {
                SerializedProperty propertyBase = this.m_Property
                    .FindPropertyRelative(ItemSocketDrawer.PROP_BASE);

                SerializedProperty propertySocketID = this.m_Property
                    .FindPropertyRelative(ItemSocketDrawer.PROP_SOCKET_ID);
                
                string valueBase = propertyBase.objectReferenceValue != null
                    ? propertyBase.objectReferenceValue.name
                    : "(none)";

                string valueSocketID = propertySocketID
                    .FindPropertyRelative(IdStringDrawer.NAME_STRING)
                    .stringValue;

                if (string.IsNullOrEmpty(valueSocketID)) valueSocketID = "none";
                return $"{valueBase} ({valueSocketID})";
            }
        }

        protected override Color Color => ColorTheme.Get(ColorTheme.Type.TextNormal);

        protected override object Value => null;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public SocketTool(IPolymorphicListTool parentTool, int index)
            : base(parentTool, index)
        { }

        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        protected override Texture2D GetIcon()
        {
            return ICON_SOCKET.Texture;
        }
    }
}