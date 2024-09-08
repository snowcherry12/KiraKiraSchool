using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    public class BeliefsTool : TPolymorphicListTool
    {
        private const string NAME_BUTTON_ADD = "GC-Beliefs-Foot-Add";

        private static readonly IIcon ICON_ADD = new IconBelief(ColorTheme.Type.TextLight);

        // MEMBERS: -------------------------------------------------------------------------------

        protected Button m_ButtonAdd;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string ElementNameHead => "GC-Beliefs-Head";
        protected override string ElementNameBody => "GC-Beliefs-Body";
        protected override string ElementNameFoot => "GC-Beliefs-Foot";

        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Behavior/Editor/StyleSheets/Beliefs"
        };

        public override bool AllowReordering => true;
        public override bool AllowDuplicating => true;
        public override bool AllowDeleting  => true;
        public override bool AllowContextMenu => false;
        public override bool AllowCopyPaste => false;
        public override bool AllowInsertion => false;
        public override bool AllowBreakpoint => false;
        public override bool AllowDisable => false;
        public override bool AllowDocumentation => false;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public BeliefsTool(SerializedProperty property) : base(property, BeliefsDrawer.PROP_LIST)
        {
            this.SerializedObject.Update();
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override VisualElement MakeItemTool(int index)
        {
            return new BeliefTool(this, index);
        }

        protected override void SetupHead()
        { }

        protected override void SetupFoot()
        {
            base.SetupFoot();
            
            this.m_ButtonAdd = new Button { name = NAME_BUTTON_ADD };

            this.m_ButtonAdd.Add(new Image { image = ICON_ADD.Texture });
            this.m_ButtonAdd.Add(new Label { text = "Add Belief..." });

            this.m_ButtonAdd.clicked += () =>
            {
                this.SerializedObject.Update();
            
                int insertIndex = this.PropertyList.arraySize;
                this.PropertyList.InsertArrayElementAtIndex(insertIndex);
                this.PropertyList
                    .GetArrayElementAtIndex(insertIndex)
                    .SetValue(new Belief());

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