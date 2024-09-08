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
    [CustomEditor(typeof(AttributeUI))]
    public class AttributeUIEditor : UnityEditor.Editor
    {
        // MEMBERS: -------------------------------------------------------------------------------

        private VisualElement m_Root;
        private VisualElement m_Head;
        private VisualElement m_Body;

        private SerializedProperty m_WhenIncrement;
        private SerializedProperty m_WhenDecrement;
        private SerializedProperty m_TransitionDuration;
        private SerializedProperty m_StallDuration;
        
        private VisualElement m_TransitionOptions;

        // PAINT METHOD: --------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            this.m_Head = new VisualElement();
            this.m_Body = new VisualElement();
            
            this.m_Root.Add(this.m_Head);
            this.m_Root.Add(this.m_Body);

            SerializedProperty attrTarget = this.serializedObject.FindProperty("m_Target");
            SerializedProperty attrAsset = this.serializedObject.FindProperty("m_Attribute");
            
            SerializedProperty common = this.serializedObject.FindProperty("m_Common");
            
            SerializedProperty attrValue = this.serializedObject.FindProperty("m_Value");
            SerializedProperty attrPercentage = this.serializedObject.FindProperty("m_Percentage");
            SerializedProperty attrMinValue = this.serializedObject.FindProperty("m_MinValue");
            SerializedProperty attrMaxValue = this.serializedObject.FindProperty("m_MaxValue");

            SerializedProperty attrImageFill = this.serializedObject.FindProperty("m_ImageFill");
            SerializedProperty attrImageScaleX = this.serializedObject.FindProperty("m_ScaleX");
            SerializedProperty attrImageScaleY = this.serializedObject.FindProperty("m_ScaleY");
            
            SerializedProperty unitContainer = this.serializedObject.FindProperty("m_UnitContainer");
            SerializedProperty unitPrefab = this.serializedObject.FindProperty("m_UnitPrefab");
            SerializedProperty unitMode = this.serializedObject.FindProperty("m_UnitMode");
            SerializedProperty unitValue = this.serializedObject.FindProperty("m_UnitValue");


            this.m_WhenIncrement = this.serializedObject.FindProperty("m_WhenIncrement");
            this.m_WhenDecrement = this.serializedObject.FindProperty("m_WhenDecrement");
            this.m_StallDuration = this.serializedObject.FindProperty("m_StallDuration");
            this.m_TransitionDuration = this.serializedObject.FindProperty("m_TransitionDuration");

            PropertyField fieldTarget = new PropertyField(attrTarget);
            PropertyField fieldAttribute = new PropertyField(attrAsset);
            
            this.m_Head.Add(fieldTarget);
            this.m_Head.Add(fieldAttribute);
            
            this.m_Body.Add(new SpaceSmall());
            this.m_Body.Add(new PropertyField(common));
            
            this.m_Body.Add(new SpaceSmall());
            this.m_Body.Add(new LabelTitle("Values:"));
            this.m_Body.Add(new PropertyField(attrValue));
            this.m_Body.Add(new PropertyField(attrPercentage));
            this.m_Body.Add(new PropertyField(attrMinValue));
            this.m_Body.Add(new PropertyField(attrMaxValue));

            this.m_Body.Add(new SpaceSmall());
            this.m_Body.Add(new LabelTitle("Progress:"));
            this.m_Body.Add(new PropertyField(attrImageFill));
            this.m_Body.Add(new PropertyField(attrImageScaleX));
            this.m_Body.Add(new PropertyField(attrImageScaleY));
            
            this.m_Body.Add(new SpaceSmall());
            this.m_Body.Add(new LabelTitle("Units:"));
            this.m_Body.Add(new PropertyField(unitContainer));
            this.m_Body.Add(new PropertyField(unitPrefab));
            this.m_Body.Add(new PropertyField(unitMode));
            this.m_Body.Add(new PropertyField(unitValue));

            this.m_Body.Add(new SpaceSmall());
            this.m_Body.Add(new LabelTitle("Transitions:"));

            PropertyField fieldWhenIncrement = new PropertyField(this.m_WhenIncrement);
            PropertyField fieldWhenDecrement = new PropertyField(this.m_WhenDecrement);
            
            this.m_Body.Add(fieldWhenIncrement);
            this.m_Body.Add(fieldWhenDecrement);
            
            this.m_TransitionOptions = new VisualElement();
            this.m_Body.Add(this.m_TransitionOptions);

            fieldWhenIncrement.RegisterValueChangeCallback(_ => this.UpdateTransitionOptions());
            fieldWhenDecrement.RegisterValueChangeCallback(_ => this.UpdateTransitionOptions());

            this.UpdateTransitionOptions();

            this.m_Body.SetEnabled(attrAsset.objectReferenceValue != null);
            fieldAttribute.RegisterValueChangeCallback(changeEvent =>
            {
                bool exists = changeEvent.changedProperty.objectReferenceValue != null;
                this.m_Body.SetEnabled(exists);
            });
            
            return this.m_Root;
        }

        private void UpdateTransitionOptions()
        {
            this.serializedObject.Update();
            this.m_TransitionOptions.Clear();
                
            if (!this.m_WhenIncrement.boolValue && !this.m_WhenDecrement.boolValue) return;

            PropertyField fieldStallDuration = new PropertyField(this.m_StallDuration);
            PropertyField fieldTransitionDuration = new PropertyField(this.m_TransitionDuration);

            this.m_TransitionOptions.Add(fieldStallDuration);
            this.m_TransitionOptions.Add(fieldTransitionDuration);

            fieldStallDuration.Bind(this.serializedObject);
            fieldTransitionDuration.Bind(this.serializedObject);
        }
        
        // CREATION MENU: -------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/UI/Stats/Attribute UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "Attribute UI";

            UnityEngine.UI.Image image = gameObject.GetComponent<UnityEngine.UI.Image>();
            image.fillMethod = UnityEngine.UI.Image.FillMethod.Horizontal;
            image.fillAmount = 0.75f;

            AttributeUI.CreateFrom(image);

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}