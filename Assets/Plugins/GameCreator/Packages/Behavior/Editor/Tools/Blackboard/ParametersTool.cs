using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    internal class ParametersTool : VisualElement
    {
        private const string INPUT_TEXT = "Type parameter name...";
        
        private const string NAME_HEAD = "GC-Overlays-Blackboard-Head";
        private const string NAME_BODY = "GC-Overlays-Blackboard-Body";
        
        // MEMBERS: -------------------------------------------------------------------------------

        private readonly SerializedProperty m_PropertyData;
        
        private readonly VisualElement m_Head;
        private readonly ScrollView m_Body;

        private readonly TextField m_CreateInput;
        private readonly Button m_CreateButton;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedObject SerializedObject => this.m_PropertyData.serializedObject;

        private SerializedProperty PropertyList => this.m_PropertyData.FindPropertyRelative("m_List");
        
        public ManipulatorBlackboardSort ManipulatorSort { get; }

        public List<ParameterTool> Parameters
        {
            get
            {
                IEnumerable<VisualElement> children = this.m_Body.Children();
                List<ParameterTool> result = new List<ParameterTool>(this.m_Body.childCount);

                foreach (VisualElement child in children)
                {
                    if (child is not ParameterTool parameter) continue;
                    result.Add(parameter);
                }

                return result;
            }
        }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public ParametersTool(SerializedProperty propertyData)
        {
            this.m_PropertyData = propertyData;
            
            this.m_Head = new VisualElement { name = NAME_HEAD };
            this.m_Body = new ScrollView
            {
                name = NAME_BODY,
                mode = ScrollViewMode.VerticalAndHorizontal
            };
            
            this.Add(this.m_Head);
            this.Add(this.m_Body);

            EditorApplication.playModeStateChanged -= this.OnChangePlayMode;
            EditorApplication.playModeStateChanged += this.OnChangePlayMode;

            this.m_CreateInput = new TextField(string.Empty) { value = INPUT_TEXT };
            this.m_CreateButton = new Button(this.CreateParameter) { text = "+" };
            
            this.m_Head.Add(this.m_CreateInput);
            this.m_Head.Add(this.m_CreateButton);
            
            this.m_CreateInput.RegisterValueChangedCallback(changeEvent =>
            {
                this.m_CreateInput.value = TextUtils.ProcessID(changeEvent.newValue);
            });
            
            this.m_CreateInput.RegisterCallback((KeyDownEvent keyDownEvent) =>
            {
                if (keyDownEvent.keyCode != KeyCode.Return) return;
                if (string.IsNullOrEmpty(this.m_CreateInput.value)) return;
                
                this.CreateParameter();
                this.m_CreateInput.Focus();
            });

            this.ManipulatorSort = new ManipulatorBlackboardSort(this);
            
            this.RefreshHead();
            this.RefreshBody();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Refresh()
        {
            this.RefreshBody();
        }

        public int GetParameterIndex(ParameterTool item)
        {
            for (int i = 0; i < this.m_Body.childCount; ++i)
            {
                VisualElement child = this.m_Body[i];
                if (child == item) return i;
            }

            return -1;
        }
        
        public void MoveParameters(int sourceIndex, int targetIndex)
        {
            this.SerializedObject.Update();
            
            if (sourceIndex < targetIndex) targetIndex -= 1;
            this.PropertyList.MoveArrayElement(sourceIndex, targetIndex);
            
            this.SerializedObject.ApplyModifiedPropertiesWithoutUndo();
            
            this.RefreshBody();
            this.SendChangeEvent();
        }

        public void RefreshDragUI(int sourceIndex, int targetIndex)
        {
            List<ParameterTool> parameters = this.Parameters;
            if (parameters.Count <= 0) return;
            
            foreach (ParameterTool parameter in parameters)
            {
                parameter.DisplayAsNormal();
            }

            if (sourceIndex != -1)
            {
                parameters[sourceIndex].DisplayAsDrag();
            }

            if (targetIndex != -1)
            {
                if (targetIndex < parameters.Count)
                {
                    parameters[targetIndex].DisplayAsTargetAbove();
                }
                else
                {
                    parameters[^1].DisplayAsTargetBelow();
                }
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshHead()
        {
            this.m_Head.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);
        }

        private void RefreshBody()
        {
            this.m_Body.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);
            this.m_Body.Clear();
            
            this.SerializedObject.Update();
            SerializedProperty list = this.PropertyList;
            
            for (int i = 0; i < list.arraySize; ++i)
            {
                ParameterTool parameterTool = new ParameterTool(this, list, i);
                this.m_Body.Add(parameterTool);
            }
        }
        
        // CALLBACKS: -----------------------------------------------------------------------------

        private void OnChangePlayMode(PlayModeStateChange playModeStateChange)
        {
            this.RefreshHead();
            this.RefreshBody();
        }

        private void CreateParameter()
        {
            string parameterId = TextUtils.ProcessID(this.m_CreateInput?.value);
            
            if (string.IsNullOrEmpty(parameterId)) return;
            if (parameterId == TextUtils.ProcessID(INPUT_TEXT))
            {
                this.m_CreateInput?.Focus();
                return;
            }
            
            Parameter parameter = new Parameter(parameterId, new ValueNull());
            this.SerializedObject.Update();
            
            int index = this.PropertyList.arraySize;
            this.PropertyList.InsertArrayElementAtIndex(index);
            this.PropertyList.GetArrayElementAtIndex(index).SetValue(parameter);

            SerializationUtils.ApplyUnregisteredSerialization(this.SerializedObject);
            
            this.RefreshBody();
            this.SendChangeEvent();
        }

        public void SendChangeEvent()
        {
            using SerializedPropertyChangeEvent changeEvent = SerializedPropertyChangeEvent.GetPooled(
                this.PropertyList
            );
            
            changeEvent.target = this.parent;
            this.parent.SendEvent(changeEvent);
        }
    }
}