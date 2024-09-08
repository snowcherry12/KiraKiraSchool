using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    public class CombosToolInspector : VisualElement
    {
        private const string KEY_STATE = "gc:melee:state-inspector";
        private const string KEY_SLIDER = "gc:melee:slider-inspector";
        
        private const string TEXT_EMPTY = "No selection";
        private const string NAME_EMPTY = "GC-Melee-Inspector-Empty";
        private const string NAME_CONTENT = "GC-Melee-Inspector-Content";
        
        private const string NAME_SCROLL = "GC-Melee-Inspector-Scroll";

        private const float MIN_DEFAULT_WIDTH = 300f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        private CombosToolInspectorNode m_NodeTool;
        
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
        
        private CombosTool CombosTool { get; }
        
        // EVENTS: --------------------------------------------------------------------------------
        
        public event Action EventState;
        public event Action EventChange;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public CombosToolInspector(CombosTool combosTool)
        {
            this.CombosTool = combosTool;

            this.m_Scroll = new ScrollView { name = NAME_SCROLL };
            this.m_Scroll.contentContainer.AddToClassList(AlignLabel.CLASS_UNITY_MAIN_CONTAINER);

            this.Add(this.m_Scroll);

            this.RegisterCallback<GeometryChangedEvent>(this.OnChangeGeometry);
            this.OnChangeSelection(ComboTree.NODE_INVALID);
        }

        public void Setup()
        {
            this.CombosTool.Tree.EventSelection += this.OnChangeSelection;
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnChangeGeometry(GeometryChangedEvent eventGeometry)
        {
            if (this.CombosTool == null) return;
            float width = eventGeometry.newRect.width;
            
            if (Math.Abs(width) < MIN_DEFAULT_WIDTH) return;
            this.Slider = width;
        }

        private void OnChangeSelection(int nodeId)
        {
            this.CombosTool.SerializedObject.Update();
            
            this.m_Scroll.Clear();
            
            if (nodeId == ComboTree.NODE_INVALID)
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
                SerializedProperty node = this.CombosTool.FindPropertyForId(nodeId);
                if (node == null)
                {
                    this.Add(new Label
                    {
                        text = TEXT_EMPTY,
                        name = NAME_EMPTY
                    });
                    
                    return;
                }

                bool isRoot = this.CombosTool.Instance.IsRoot(nodeId);
                VisualElement container = new VisualElement { name = NAME_CONTENT };
                
                this.m_NodeTool = new CombosToolInspectorNode(this.CombosTool, node, isRoot);
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