using System;
using GameCreator.Runtime.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Quests
{
    public class TasksToolInspector : VisualElement
    {
        private const string KEY_STATE = "gc:quests:state-inspector";
        private const string KEY_SLIDER = "gc:quests:slider-inspector";
        
        private const string TEXT_EMPTY = "No selection";
        private const string NAME_EMPTY = "GC-Quests-Inspector-Empty";
        private const string NAME_CONTENT = "GC-Quests-Inspector-Content";
        
        private const string NAME_SCROLL = "GC-Quests-Inspector-Scroll";

        private const float MIN_DEFAULT_WIDTH = 300f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        private TasksToolInspectorNode m_NodeTool;
        
        private readonly ScrollView m_Scroll;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool State
        {
            get => EditorPrefs.GetBool(KEY_STATE, false);
            set
            {
                EditorPrefs.SetBool(KEY_STATE, value);
                this.EventState?.Invoke();
            }
        }

        public float Slider
        {
            get => EditorPrefs.GetFloat(KEY_SLIDER, MIN_DEFAULT_WIDTH);
            set => EditorPrefs.SetFloat(KEY_SLIDER, value);
        }
        
        private TasksTool TasksTool { get; }
        
        // EVENTS: --------------------------------------------------------------------------------
        
        public event Action EventState;
        public event Action EventChange;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public TasksToolInspector(TasksTool tasksTool)
        {
            this.TasksTool = tasksTool;
            this.m_Scroll = new ScrollView { name = NAME_SCROLL };
            
            this.Add(this.m_Scroll);

            this.RegisterCallback<GeometryChangedEvent>(this.OnChangeGeometry);
            this.OnChangeSelection(TasksTree.NODE_INVALID);
        }

        public void Setup()
        {
            this.TasksTool.Tree.EventSelection += this.OnChangeSelection;
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnChangeGeometry(GeometryChangedEvent eventGeometry)
        {
            if (this.TasksTool == null) return;
            float width = eventGeometry.newRect.width;
            
            if (Math.Abs(width) < MIN_DEFAULT_WIDTH) return;
            this.Slider = width;
        }

        private void OnChangeSelection(int nodeId)
        {
            this.TasksTool.SerializedObject.Update();
            
            this.m_Scroll.Clear();
            
            if (nodeId == TasksTree.NODE_INVALID)
            {
                this.m_NodeTool = null;
                this.m_Scroll.Add(new Label
                {
                    text = TEXT_EMPTY,
                    name = NAME_EMPTY
                });
            }
            else
            {
                SerializedProperty node = this.TasksTool.FindPropertyForId(nodeId);
                if (node == null)
                {
                    this.Add(new Label
                    {
                        text = TEXT_EMPTY,
                        name = NAME_EMPTY
                    });
                    
                    return;
                }

                VisualElement container = new VisualElement { name = NAME_CONTENT };
                this.m_NodeTool = new TasksToolInspectorNode(this.TasksTool, node);
                this.m_NodeTool.RegisterCallback<SerializedPropertyChangeEvent>(_ =>
                {
                    this.EventChange?.Invoke();
                });

                container.Add(this.m_NodeTool);
                this.m_Scroll.Add(container);
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void ToggleState()
        {
            bool state = this.State;
            this.State = !state;
        }
    }
}