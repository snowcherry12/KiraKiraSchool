using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace GameCreator.Editor.Dialogue
{
    public class ExpressionTool : TPolymorphicItemTool
    {
        private static readonly IIcon ICON_EXPRESSION = new IconExpression(ColorTheme.Type.Green);
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Dialogue/Editor/StyleSheets/Expression-Head",
            EditorPaths.PACKAGES + "Dialogue/Editor/StyleSheets/Expression-Body"
        };

        protected override object Value => null;

        public override string Title
        {
            get
            {
                SerializedProperty id = this.m_Property.FindPropertyRelative(Expression.NAME_ID);
                string text = id?.FindPropertyRelative(IdStringDrawer.NAME_STRING)?.stringValue;

                return this.Index == 0 ? $"{text} [default]" : text;
            }
        }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ExpressionTool(IPolymorphicListTool parentTool, int index)
            : base(parentTool, index)
        { }
        
        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        protected override Texture2D GetIcon()
        {
            return ICON_EXPRESSION.Texture;
        }

        protected override void SetupBody()
        {
            PropertyField field = new PropertyField(this.m_Property);
            field.Bind(this.m_Property.serializedObject);
            
            field.RegisterValueChangeCallback(_ => this.UpdateHead());
            
            this.m_Body.Add(field);
            this.UpdateBody(false);
        }
    }
}