using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    public class PropertiesTool : TPolymorphicListTool
    {
        private const string NAME_BUTTON_ADD = "GC-Inventory-Item-Properties-Foot-Add";
        
        private static readonly IIcon ICON_ADD = new IconProperty(ColorTheme.Type.TextLight);
        
        // MEMBERS: -------------------------------------------------------------------------------

        private Button m_ButtonAdd;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string ElementNameHead => "GC-Inventory-Item-Properties-Head";
        protected override string ElementNameBody => "GC-Inventory-Item-Properties-Body";
        protected override string ElementNameFoot => "GC-Inventory-Item-Properties-Foot";

        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Properties",
        };
        
        public override bool AllowReordering => true;
        public override bool AllowDuplicating => false;
        public override bool AllowDeleting => true;
        public override bool AllowContextMenu => false;
        public override bool AllowCopyPaste => false;
        public override bool AllowInsertion => false;
        public override bool AllowBreakpoint => false;
        public override bool AllowDisable => false;
        public override bool AllowDocumentation => false;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public PropertiesTool(SerializedProperty property, string listName) 
            : base(property, listName)
        { }
        
        // IMPLEMENT METHODS: ---------------------------------------------------------------------

        protected override VisualElement MakeItemTool(int index)
        {
            return new PropertyTool(this, index);
        }
        
        protected override void SetupFoot()
        {
            base.SetupFoot();

            this.m_ButtonAdd = new Button(() =>
            {
                this.SerializedObject.Update();
            
                int insertIndex = this.PropertyList.arraySize;
                this.PropertyList.InsertArrayElementAtIndex(insertIndex);
                this.PropertyList
                    .GetArrayElementAtIndex(insertIndex)
                    .FindPropertyRelative(ItemPropertyDrawer.PROP_TEXT)
                    .FindPropertyRelative(IPropertyDrawer.PROPERTY_NAME)
                    .SetValue(new GetStringString());

                SerializationUtils.ApplyUnregisteredSerialization(this.SerializedObject);

                int size = this.PropertyList.arraySize;
                this.ExecuteEventChangeSize(size);
            
                using ChangeEvent<int> changeEvent = ChangeEvent<int>.GetPooled(size, size);
                changeEvent.target = this;
                this.SendEvent(changeEvent);
            
                this.Refresh();
            });

            this.m_ButtonAdd.name = NAME_BUTTON_ADD;
            this.m_ButtonAdd.Add(new Image { image = ICON_ADD.Texture });
            this.m_ButtonAdd.Add(new Label { text = "Add Property..." });

            this.m_Foot.Add(this.m_ButtonAdd);
        }
    }
}