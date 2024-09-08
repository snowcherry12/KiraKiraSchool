using GameCreator.Editor.Common;
using GameCreator.Editor.Common.UnityUI;
using GameCreator.Runtime.Stats.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats.UnityUI
{
    [CustomEditor(typeof(StatUI))]
    public class StatUIEditor : UnityEditor.Editor
    {
        // MEMBERS: -------------------------------------------------------------------------------

        private VisualElement m_Root;
        private VisualElement m_Head;
        private VisualElement m_Body;
        
        // PAINT METHOD: --------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            this.m_Head = new VisualElement();
            this.m_Body = new VisualElement();
            
            this.m_Root.Add(this.m_Head);
            this.m_Root.Add(this.m_Body);

            SerializedProperty statTarget = this.serializedObject.FindProperty("m_Target");
            SerializedProperty statAsset = this.serializedObject.FindProperty("m_Stat");
            
            SerializedProperty common = this.serializedObject.FindProperty("m_Common");
            
            SerializedProperty statValue = this.serializedObject.FindProperty("m_Value");
            SerializedProperty statBase = this.serializedObject.FindProperty("m_Base");
            SerializedProperty statModifiers = this.serializedObject.FindProperty("m_Modifiers");
            SerializedProperty ratioFill = this.serializedObject.FindProperty("m_RatioFill");
            
            SerializedProperty activeIfModifiers = this.serializedObject.FindProperty("m_ActiveIfModifiers");
            SerializedProperty activeIfNotModifiers = this.serializedObject.FindProperty("m_ActiveIfNotModifiers");

            PropertyField fieldTarget = new PropertyField(statTarget);
            PropertyField fieldStat = new PropertyField(statAsset);
            
            this.m_Head.Add(fieldTarget);
            this.m_Head.Add(fieldStat);
            
            this.m_Body.Add(new SpaceSmall());
            this.m_Body.Add(new PropertyField(common));
            
            this.m_Body.Add(new SpaceSmall());
            this.m_Body.Add(new LabelTitle("Values:"));
            this.m_Body.Add(new PropertyField(statValue));
            this.m_Body.Add(new PropertyField(statBase));
            this.m_Body.Add(new PropertyField(statModifiers));
            this.m_Body.Add(new PropertyField(ratioFill));
            
            this.m_Body.Add(new SpaceSmall());
            this.m_Body.Add(new PropertyField(activeIfModifiers));
            this.m_Body.Add(new PropertyField(activeIfNotModifiers));
            
            this.m_Body.SetEnabled(statAsset.objectReferenceValue != null);
            fieldStat.RegisterValueChangeCallback(changeEvent =>
            {
                bool exists = changeEvent.changedProperty.objectReferenceValue != null;
                this.m_Body.SetEnabled(exists);
            });
            
            return this.m_Root;
        }
        
        // CREATION MENU: -------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/UI/Stats/Stat UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateText(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "Stat UI";

            Text text = gameObject.GetComponent<Text>();
            text.text = "99";
            
            StatUI.CreateFrom(text);

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}