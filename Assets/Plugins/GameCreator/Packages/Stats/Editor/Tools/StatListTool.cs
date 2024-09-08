using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    public class StatListTool : TPolymorphicListTool
    {
        private const string NAME_BUTTON_ADD = "GC-Stat-List-Foot-Add";
        
        private static readonly IIcon ICON_ADD = new IconStat(ColorTheme.Type.TextLight);

        // MEMBERS: -------------------------------------------------------------------------------

        private Button m_ButtonAdd;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string ElementNameHead => "GC-Stat-List-Head";
        protected override string ElementNameBody => "GC-Stat-List-Body";
        protected override string ElementNameFoot => "GC-Stat-List-Foot";
        
        protected override List<string> CustomStyleSheetPaths => new List<string>
        {
            EditorPaths.PACKAGES + "Stats/Editor/StyleSheets/Stat-List"
        };

        public override bool AllowReordering => true;
        public override bool AllowDuplicating => false;
        public override bool AllowDeleting  => true;
        public override bool AllowContextMenu => false;
        public override bool AllowInsertion => false;
        public override bool AllowCopyPaste => false;
        public override bool AllowBreakpoint => false;
        public override bool AllowDisable => false;
        public override bool AllowDocumentation => false;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public StatListTool(SerializedProperty property)
            : base(property, "m_Stats")
        {
            this.SerializedObject.Update();
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override VisualElement MakeItemTool(int index)
        {
            return new StatItemTool(this, index);
        }

        protected override void SetupHead()
        { }

        protected override void SetupFoot()
        {
            base.SetupFoot();

            this.m_ButtonAdd = new Button(() =>
            {
                int insertIndex = this.PropertyList.arraySize;
                this.InsertItem(insertIndex, new StatItem());
            });

            this.m_ButtonAdd.name = NAME_BUTTON_ADD;
            this.m_ButtonAdd.Add(new Image { image = ICON_ADD.Texture });
            this.m_ButtonAdd.Add(new Label { text = "Add Stat..." });

            this.m_Foot.Add(this.m_ButtonAdd);
        }
    }
}