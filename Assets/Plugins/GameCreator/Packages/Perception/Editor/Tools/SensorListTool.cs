using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    public class SensorListTool : TPolymorphicListTool
    {
        private const string NAME_BUTTON_ADD = "GC-SensorList-Foot-Add";

        // MEMBERS: -------------------------------------------------------------------------------

        private Button m_ButtonAdd;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string ElementNameHead => "GC-SensorList-Head";
        protected override string ElementNameBody => "GC-SensorList-Body";
        protected override string ElementNameFoot => "GC-SensorList-Foot";

        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Perception/Editor/StyleSheets/SensorList"
        };

        public override bool AllowReordering => true;
        public override bool AllowDuplicating => false;
        public override bool AllowDeleting  => true;
        public override bool AllowContextMenu => true;
        public override bool AllowCopyPaste => false;
        public override bool AllowInsertion => false;
        public override bool AllowBreakpoint => false;
        public override bool AllowDisable => true;
        public override bool AllowDocumentation => true;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public SensorListTool(SerializedProperty property) : base(property, "m_Sensors")
        {
            this.SerializedObject.Update();
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override VisualElement MakeItemTool(int index)
        {
            return new SensorTool(this, index);
        }

        protected override void SetupHead()
        { }

        protected override void SetupFoot()
        {
            base.SetupFoot();
            
            this.m_ButtonAdd = new TypeSelectorElementSensor(this.PropertyList, this)
            {
                name = NAME_BUTTON_ADD
            };

            this.m_Foot.Add(this.m_ButtonAdd);
        }
    }
}