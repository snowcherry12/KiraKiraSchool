using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomEditor(typeof(Runtime.Perception.Perception))]
    public class PerceptionEditor : UnityEditor.Editor
    {
        private const string USS_PATH = EditorPaths.PACKAGES + "Perception/Editor/StyleSheets/Perception";
        
        private const string NAME_ROOT = "GC-Perception-Root";
        private const string NAME_HEAD = "GC-Perception-Head";
        private const string NAME_BODY = "GC-Perception-Body";
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private VisualElement m_Root;
        [NonSerialized] private VisualElement m_Head;
        [NonSerialized] private VisualElement m_Body;

        // INSPECTOR METHODS: ---------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement { name = NAME_ROOT };
            this.m_Head = new VisualElement { name = NAME_HEAD };
            this.m_Body = new VisualElement { name = NAME_BODY };

            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets) this.m_Root.styleSheets.Add(sheet);
            
            this.m_Root.Add(this.m_Head);
            this.m_Root.Add(this.m_Body);

            SerializedProperty canForget = this.serializedObject.FindProperty("m_CanForget");
            SerializedProperty duration = this.serializedObject.FindProperty("m_Duration");
            
            PropertyField fieldCanForget = new PropertyField(canForget);
            PropertyField fieldAwareDuration = new PropertyField(duration)
            {
                style = { marginLeft = 10 }
            };
            
            this.m_Head.Add(new SpaceSmaller());
            this.m_Head.Add(fieldCanForget);
            this.m_Head.Add(fieldAwareDuration);
            
            fieldCanForget.RegisterValueChangeCallback(changeEvent =>
            {
                fieldAwareDuration.style.display = changeEvent.changedProperty.boolValue
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });
            
            fieldAwareDuration.style.display = canForget.boolValue
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            SerializedProperty forgetSpeed = this.serializedObject.FindProperty("m_ForgetSpeed");
            SerializedProperty forgetDelay = this.serializedObject.FindProperty("m_ForgetDelay");
            SerializedProperty sensors = this.serializedObject.FindProperty("m_Sensors");
            
            this.m_Head.Add(new SpaceSmaller());
            this.m_Head.Add(new PropertyField(forgetSpeed));
            this.m_Head.Add(new SpaceSmaller());
            this.m_Head.Add(new PropertyField(forgetDelay));
            this.m_Head.Add(new SpaceSmall());
            this.m_Head.Add(new PropertyField(sensors));

            if (EditorApplication.isPlayingOrWillChangePlaymode && 
                !PrefabUtility.IsPartOfPrefabAsset(this.target))
            {
                Runtime.Perception.Perception perception = this.target as Runtime.Perception.Perception;
                PerceptionView perceptionRuntime = new PerceptionView(perception);
                
                this.m_Body.Add(perceptionRuntime);
            }
            
            return this.m_Root;
        }
    }   
}
