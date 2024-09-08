using GameCreator.Editor.Common;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [Overlay(
        id = ID,
        displayName = NAME,
        editorWindowType = typeof(WindowStateMachine),
        defaultDisplay = true,
        defaultDockZone = DockZone.RightColumn,
        defaultDockPosition = DockPosition.Top,
        defaultDockIndex = 1,
        defaultLayout = Layout.Panel
    )]
    
    internal class InspectorStateMachine : TInspector
    {
        private const string TITLE_TRANSITIONS = "Transitions:";

        private const string NAME_TRANSITIONS = "GC-Overlay-Graph-Inspector-Body-Transitions";
        
        // MEMBERS: -------------------------------------------------------------------------------

        private SerializedProperty m_ConnectionsProperty;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override Layout supportedLayouts => Layout.Panel;

        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected override void Refresh()
        {
            base.Refresh();

            this.m_ConnectionsProperty = null;
            
            TGraphTool currentPage = this.GraphWindow.CurrentPage;
            if (currentPage == null) return;

            TNodeTool[] selection = this.GraphWindow.Selection.Group;
            if (selection.Length != 1) return;

            if (selection[0] is not TNodeToolStateMachine selected) return;
            if (!selected.ShowInspectorTransitions) return;
            
            this.m_ConnectionsProperty = selected.OutputPortsProperty.arraySize == 1
                ? selected.OutputPortsProperty
                    .GetArrayElementAtIndex(0)
                    .FindPropertyRelative(TPortTool.PROP_CONNECTIONS)
                : null;
            
            if (this.m_ConnectionsProperty == null) return;

            ListView listView = new ListView
            {
                name = NAME_TRANSITIONS,
                bindingPath = this.m_ConnectionsProperty.propertyPath,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                showAlternatingRowBackgrounds = AlternatingRowBackground.All,
                reorderable = true,
                reorderMode = ListViewReorderMode.Animated,
                showFoldoutHeader = false,
                showAddRemoveFooter = false,
                showBoundCollectionSize = false,
                makeItem = this.MakeTransitionItem,
                bindItem = this.BindTransitionItem
            };
            
            listView.Bind(selected.Property.serializedObject);
            
            this.m_Body.Add(new SpaceSmall());
            this.m_Body.Add(new LabelTitle(TITLE_TRANSITIONS));
            this.m_Body.Add(new SpaceSmaller());
            this.m_Body.Add(listView);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private VisualElement MakeTransitionItem()
        {
            return new ConnectionTool(this.GraphWindow.CurrentPage);
        }
        
        private void BindTransitionItem(VisualElement item, int index)
        {
            if (item is not ConnectionTool connectionTool) return;
            if (this.m_ConnectionsProperty == null) return;
            
            connectionTool.Refresh(this.m_ConnectionsProperty, index);
        }
    }
}