using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    public class ContentToolInspector : VisualElement
    {
        private const string KEY_STATE = "gc:dialogue:state-inspector";
        private const string KEY_SLIDER = "gc:dialogue:slider-inspector";
        
        private const string TEXT_EMPTY = "No selection";
        private const string NAME_EMPTY = "GC-Dialogue-Inspector-Empty";
        private const string NAME_CONTENT = "GC-Dialogue-Inspector-Content";
        
        private const string NAME_SCROLL = "GC-Dialogue-Inspector-Scroll";
        
        private const float MIN_DEFAULT_WIDTH = 300f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        private ContentToolInspectorNode m_NodeTool;
        
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
        
        private ContentTool ContentTool { get; }
        
        // EVENTS: --------------------------------------------------------------------------------
        
        public event Action EventState;

        public event Action EventChange;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public ContentToolInspector(ContentTool contentTool)
        {
            this.ContentTool = contentTool;
            this.m_Scroll = new ScrollView { name = NAME_SCROLL };

            this.Add(this.m_Scroll);

            this.RegisterCallback<GeometryChangedEvent>(this.OnChangeGeometry);
            this.OnChangeSelection(Content.NODE_INVALID);
        }

        public void Setup()
        {
            this.ContentTool.Tree.EventSelection += this.OnChangeSelection;
            this.ContentTool.Settings.EventChangeActor += this.OnChangeActorFromSettings;
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnChangeGeometry(GeometryChangedEvent eventGeometry)
        {
            if (this.ContentTool == null) return;
            float width = eventGeometry.newRect.width;
            
            if (Math.Abs(width) < MIN_DEFAULT_WIDTH) return;
            this.Slider = width;
        }

        private void OnChangeSelection(int nodeId)
        {
            this.ContentTool.SerializedObject.Update();
            
            this.m_Scroll.Clear();
            
            if (nodeId == Content.NODE_INVALID)
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
                SerializedProperty node = this.ContentTool.FindPropertyForId(nodeId);
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
                this.m_NodeTool = new ContentToolInspectorNode(this.ContentTool, node);
                this.m_NodeTool.RegisterCallback<SerializedPropertyChangeEvent>(_ =>
                {
                    this.EventChange?.Invoke();
                });

                container.Add(this.m_NodeTool);
                this.m_Scroll.Add(container);
            }
        }
        
        private void OnChangeActorFromSettings()
        {
            Actor actor = this.m_NodeTool?.Actor;
            NodeSequenceTool sequence = this.m_NodeTool?.NodeSequence;
            
            if (sequence == null) return;
            sequence.Target = this.ContentTool.Settings.FindSceneReferenceForActor(actor);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void ToggleState()
        {
            bool state = this.State;
            this.State = !state;
        }
    }
}