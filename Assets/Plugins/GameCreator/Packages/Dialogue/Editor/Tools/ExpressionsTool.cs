using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    public class ExpressionsTool : TPolymorphicListTool
    {
        private const string NAME_BUTTON_ADD = "GC-Dialogue-Expressions-Foot-Add";
        
        private static readonly IIcon ICON_ADD = new IconExpression(ColorTheme.Type.TextLight);
        
        // MEMBERS: -------------------------------------------------------------------------------

        private Button m_ButtonAdd;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string ElementNameHead => "GC-Dialogue-Expressions-Head";
        protected override string ElementNameBody => "GC-Dialogue-Expressions-Body";
        protected override string ElementNameFoot => "GC-Dialogue-Expressions-Foot";

        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Dialogue/Editor/StyleSheets/Expressions"
        };

        public override bool AllowReordering => true;
        public override bool AllowDuplicating => false;
        public override bool AllowDeleting  => true;
        public override bool AllowContextMenu => false;
        public override bool AllowCopyPaste => false;
        public override bool AllowInsertion => false;
        public override bool AllowBreakpoint => false;
        public override bool AllowDisable => false;
        public override bool AllowDocumentation => false;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ExpressionsTool(SerializedProperty property) 
            : base(property, Expressions.NAME_EXPRESSIONS)
        {
            this.SerializedObject.Update();
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override VisualElement MakeItemTool(int index)
        {
            return new ExpressionTool(this, index);
        }

        protected override void SetupHead()
        { }

        protected override void SetupFoot()
        {
            base.SetupFoot();

            this.m_ButtonAdd = new Button { name = NAME_BUTTON_ADD };

            this.m_ButtonAdd.Add(new Image { image = ICON_ADD.Texture });
            this.m_ButtonAdd.Add(new Label { text = "Add Expression..." });

            this.m_ButtonAdd.clicked += () =>
            {
                this.SerializedObject.Update();
            
                int insertIndex = this.PropertyList.arraySize;
                this.PropertyList.InsertArrayElementAtIndex(insertIndex);
                this.PropertyList
                    .GetArrayElementAtIndex(insertIndex)
                    .SetValue(new Expression());
                
                SerializationUtils.ApplyUnregisteredSerialization(this.SerializedObject);
                
                int size = this.PropertyList.arraySize;
                this.ExecuteEventChangeSize(size);
            
                using ChangeEvent<int> changeEvent = ChangeEvent<int>.GetPooled(size, size);
                changeEvent.target = this;
                this.SendEvent(changeEvent);
            
                this.Refresh();
            };

            this.m_Foot.Add(this.m_ButtonAdd);
        }
    }
}