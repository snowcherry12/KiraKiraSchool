using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    public class ValueTool : TPolymorphicItemTool
    {
        private static readonly IIcon ICON_NODE = new IconString(ColorTheme.Type.Yellow);

        private const string TITLE = "{{{0}}} = {1}";
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Dialogue/Editor/StyleSheets/Value-Head",
            EditorPaths.PACKAGES + "Dialogue/Editor/StyleSheets/Value-Body"
        };

        protected override object Value => null;

        public override string Title => string.Format(
            TITLE, 
            this.m_Property
                .FindPropertyRelative(ValueDrawer.PROPERTY_KEY)
                .FindPropertyRelative(IdStringDrawer.NAME_STRING).stringValue,
            this.m_Property.GetValue<Value>()?.ToString() ?? "Unknown"
        );

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ValueTool(IPolymorphicListTool parentTool, int index)
            : base(parentTool, index)
        { }
        
        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        protected override Texture2D GetIcon()
        {
            return ICON_NODE.Texture;
        }

        protected override void SetupBody()
        {
            VisualElement fieldBody = ValueDrawer.MakePropertyGUI(this.m_Property);

            fieldBody.Bind(this.m_Property.serializedObject);
            fieldBody.RegisterCallback<SerializedPropertyChangeEvent>(_ =>
            {
                this.UpdateHead();
            });
            
            this.m_Body.Add(fieldBody);
            this.UpdateBody(false);
        }
    }
}