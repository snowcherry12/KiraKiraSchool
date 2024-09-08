using System;
using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    public class CoinsTool : TPolymorphicListTool
    {
        private const string NAME_BUTTON_ADD = "GC-Inventory-Coins-Foot-Add";
        
        private static readonly IIcon ICON_ADD = new IconCoin(ColorTheme.Type.TextLight);
        
        // MEMBERS: -------------------------------------------------------------------------------

        private Button m_ButtonAdd;

        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChange;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string ElementNameHead => "GC-Inventory-Coins-Head";
        protected override string ElementNameBody => "GC-Inventory-Coins-Body";
        protected override string ElementNameFoot => "GC-Inventory-Coins-Foot";

        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Coins",
        };
        
        public override bool AllowReordering => true;
        public override bool AllowDuplicating => false;
        public override bool AllowDeleting => true;
        public override bool AllowContextMenu => true;
        public override bool AllowCopyPaste => true;
        public override bool AllowInsertion => true;
        public override bool AllowBreakpoint => false;
        public override bool AllowDisable => false;
        public override bool AllowDocumentation => false;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public CoinsTool(SerializedProperty property, string listName)
            : base(property, listName)
        { }
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal void ExecuteEventChange()
        {
            this.EventChange?.Invoke();
        }
        
        // IMPLEMENT METHODS: ---------------------------------------------------------------------

        protected override VisualElement MakeItemTool(int index)
        {
            return new CoinTool(this, index);
        }

        protected override void SetupFoot()
        {
            base.SetupFoot();

            this.m_ButtonAdd = new Button(() =>
            {
                this.SerializedObject.Update();
            
                int insertIndex = this.PropertyList.arraySize;
                this.PropertyList.InsertArrayElementAtIndex(insertIndex);
                SerializedProperty insert = this.PropertyList.GetArrayElementAtIndex(insertIndex);
                insert.SetValue(new Coin());

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
            this.m_ButtonAdd.Add(new Label { text = "Add Coin..." });

            this.m_Foot.Add(this.m_ButtonAdd);
        }
    }
}