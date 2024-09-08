using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomEditor(typeof(Traits))]
    public class TraitsEditor : UnityEditor.Editor
    {
        private const string MSG_DROP_CLASS = "Select a Class asset";
        private const string USS_PATH = EditorPaths.PACKAGES + "Stats/Editor/StyleSheets/Traits";

        // MEMBERS: -------------------------------------------------------------------------------

        private VisualElement m_Root;
        private VisualElement m_Head;
        private VisualElement m_Body;

        private SerializedProperty m_PropertyClass;
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            this.m_Head = new VisualElement();
            this.m_Body = new VisualElement();

            this.m_Root.Add(this.m_Head);
            this.m_Root.Add(this.m_Body);
            
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets) this.m_Root.styleSheets.Add(sheet);

            this.m_PropertyClass = this.serializedObject.FindProperty("m_Class");
            PropertyField fieldClass = new PropertyField(this.m_PropertyClass, string.Empty);
            
            fieldClass.RegisterValueChangeCallback(_ => this.RefreshBody());
            this.RefreshBody();

            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                this.m_Head.Add(new SpaceSmaller());
                this.m_Head.Add(fieldClass);
            }
            
            return this.m_Root;
        }

        private void RefreshBody()
        {
            this.serializedObject.Update();
            this.m_Body.Clear();
            
            if (this.m_PropertyClass.objectReferenceValue == null)
            {
                this.m_Body.Add(new ErrorMessage(MSG_DROP_CLASS));
                return;
            }
            
            Class traitsClass = this.m_PropertyClass.objectReferenceValue as Class;
            if (traitsClass == null)
            {
                string error = $"Unknown Class '{this.m_PropertyClass.objectReferenceValue.name}'";
                throw new Exception(error);
            }
            
            bool playMode = EditorApplication.isPlayingOrWillChangePlaymode &&
                             !PrefabUtility.IsPartOfPrefabAsset(this.target);
            
            switch (playMode)
            {
                case true:
                    Traits traits = this.target as Traits;
                    if (traits != null)
                    {
                        this.m_Body.Add(new AttributesView(traits));
                        this.m_Body.Add(new StatsView(traits));   
                        this.m_Body.Add(new StatusEffectsView(traits));
                    }
                    break;
                
                case false:
                    SerializedProperty attrs = this.serializedObject.FindProperty("m_OverrideAttributes");
                    SerializedProperty stats = this.serializedObject.FindProperty("m_OverrideStats");
                    
                    PropertyField fieldAttrs = new PropertyField(attrs);
                    PropertyField fieldStats = new PropertyField(stats);
                    
                    this.m_Body.Add(fieldAttrs);
                    this.m_Body.Add(fieldStats);
                    
                    fieldAttrs.Bind(this.serializedObject);
                    fieldStats.Bind(this.serializedObject);
                    break;
            }
        }
    }
}