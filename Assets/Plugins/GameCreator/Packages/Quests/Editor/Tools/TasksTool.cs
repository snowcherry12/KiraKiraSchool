using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    public class TasksTool : VisualElement
    {
        private const string USS_PATH = EditorPaths.PACKAGES + "Quests/Editor/StyleSheets/TasksTool";

        private const string KEY_LASTS_SELECTION = "gc:quests:inspector-selection:{0}";

        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private readonly TwoPaneSplitView m_SplitView;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public SerializedProperty Property { get; }
        
        public TasksTree Instance
        {
            get
            {
                this.Property.serializedObject.Update();
                return this.Property.managedReferenceValue as TasksTree;
            }
            set
            {
                this.SerializedObject.Update();
                this.Property.managedReferenceValue = value;

                int random = UnityEngine.Random.Range(1, 99999);
                this.Property.FindPropertyRelative("m_Dirty").intValue = random;
                this.SerializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
        }
        
        public SerializedObject SerializedObject => this.Property.serializedObject;

        private SerializedProperty PropertyData => 
            this.Property.FindPropertyRelative(TasksTree.NAME_DATA);
        
        private SerializedProperty PropertyDataKeys => 
            this.PropertyData.FindPropertyRelative(TasksTree.NAME_DATA_KEYS);
        
        private SerializedProperty PropertyDataValues => 
            this.PropertyData.FindPropertyRelative(TasksTree.NAME_DATA_VALUES);

        public TasksToolToolbar Toolbar { get; }
        public TasksToolTree Tree { get; }
        public TasksToolInspector Inspector { get; }
        
        private int LastSelection
        {
            get => SessionState.GetInt(this.SelectionKey, TasksTree.NODE_INVALID);
            set => SessionState.SetInt(this.SelectionKey, value);
        }

        private string SelectionKey => string.Format(
            KEY_LASTS_SELECTION,
            this.Property.serializedObject.targetObject.GetInstanceID()
        );

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public TasksTool(SerializedProperty property)
        {
            this.Property = property;
            
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets) this.styleSheets.Add(sheet);

            this.Toolbar = new TasksToolToolbar(this);
            this.Tree = new TasksToolTree(this);
            this.Inspector = new TasksToolInspector(this);
            
            this.Toolbar.Setup();
            this.Tree.Setup();
            this.Inspector.Setup();
            
            int lastSelection = this.LastSelection;
            if (lastSelection != TasksTree.NODE_INVALID)
            {
                this.Tree.Select(lastSelection);
            }

            this.m_SplitView = new TwoPaneSplitView(
                1, this.Inspector.Slider,
                TwoPaneSplitViewOrientation.Horizontal
            );
            
            this.Inspector.EventState += this.RefreshSplitViews;
            this.Tree.EventSelection += this.RefreshLastSelection;

            this.Add(this.Toolbar);
            this.Add(this.m_SplitView);
            this.m_SplitView.Add(this.Tree);
            this.m_SplitView.Add(this.Inspector);

            this.RegisterCallback<GeometryChangedEvent>(_ => this.RefreshSplitViews());

            this.RefreshSplitViews();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public SerializedProperty FindPropertyForId(int id)
        {
            SerializedProperty keys = this.PropertyDataKeys;
            SerializedProperty values = this.PropertyDataValues;

            int size = keys.arraySize;
            for (int i = 0; i < size; ++i)
            {
                if (keys.GetArrayElementAtIndex(i).intValue != id) continue;
                return values.GetArrayElementAtIndex(i);
            }

            return null;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshSplitViews()
        {
            switch (this.Inspector.State)
            {
                case true: this.m_SplitView.UnCollapse(); break;
                case false: this.m_SplitView.CollapseChild(1); break;
            }
            
            this.m_SplitView.fixedPaneInitialDimension = this.Inspector.Slider;
        }
        
        private void RefreshLastSelection(int nodeId)
        {
            this.LastSelection = nodeId;
        }
    }
}