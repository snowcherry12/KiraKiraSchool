using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomEditor(typeof(CrosshairUI))]
    public class CrosshairUIEditor : UnityEditor.Editor
    {
        [NonSerialized] private SerializedProperty m_Reticle;
        [NonSerialized] private SerializedProperty m_Direction;
        [NonSerialized] private SerializedProperty m_AccuracyPosition;
        [NonSerialized] private SerializedProperty m_PositionX;
        [NonSerialized] private SerializedProperty m_PositionY;
        [NonSerialized] private SerializedProperty m_AccuracySize;
        [NonSerialized] private SerializedProperty m_Width;
        [NonSerialized] private SerializedProperty m_Height;
        [NonSerialized] private SerializedProperty m_AccuracyRotation;
        [NonSerialized] private SerializedProperty m_Rotation;
        [NonSerialized] private SerializedProperty m_AccuracyScale;
        [NonSerialized] private SerializedProperty m_ScaleX;
        [NonSerialized] private SerializedProperty m_ScaleY;
        [NonSerialized] private SerializedProperty m_AccuracyFill;
        [NonSerialized] private SerializedProperty m_Fill;
        [NonSerialized] private SerializedProperty m_AccuracyAlpha;
        [NonSerialized] private SerializedProperty m_Alpha;
        
        [NonSerialized] private PropertyField m_FieldReticle;
        [NonSerialized] private PropertyField m_FieldDirection;
        [NonSerialized] private PropertyField m_FieldAccuracyPosition;
        [NonSerialized] private CrosshairRangeField m_FieldPositionX;
        [NonSerialized] private CrosshairRangeField m_FieldPositionY;
        [NonSerialized] private PropertyField m_FieldAccuracySize;
        [NonSerialized] private CrosshairRangeField m_FieldWidth;
        [NonSerialized] private CrosshairRangeField m_FieldHeight;
        [NonSerialized] private PropertyField m_FieldAccuracyRotation;
        [NonSerialized] private CrosshairRangeField m_FieldRotation;
        [NonSerialized] private PropertyField m_FieldAccuracyScale;
        [NonSerialized] private CrosshairRangeField m_FieldScaleX;
        [NonSerialized] private CrosshairRangeField m_FieldScaleY;
        [NonSerialized] private PropertyField m_FieldAccuracyFill;
        [NonSerialized] private CrosshairRangeField m_FieldFill;
        [NonSerialized] private PropertyField m_FieldAccuracyAlpha;
        [NonSerialized] private CrosshairRangeField m_FieldAlpha;
        
        // INSPECTOR: -----------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            this.m_Reticle = this.serializedObject.FindProperty("m_Reticle");
            this.m_Direction = this.serializedObject.FindProperty("m_Direction");
            this.m_AccuracyPosition = this.serializedObject.FindProperty("m_AccuracyPosition");
            this.m_PositionX = this.serializedObject.FindProperty("m_PositionX");
            this.m_PositionY = this.serializedObject.FindProperty("m_PositionY");
            this.m_AccuracySize = this.serializedObject.FindProperty("m_AccuracySize");
            this.m_Width = this.serializedObject.FindProperty("m_Width");
            this.m_Height = this.serializedObject.FindProperty("m_Height");
            this.m_AccuracyRotation = this.serializedObject.FindProperty("m_AccuracyRotation");
            this.m_Rotation = this.serializedObject.FindProperty("m_Rotation");
            this.m_AccuracyScale = this.serializedObject.FindProperty("m_AccuracyScale");
            this.m_ScaleX = this.serializedObject.FindProperty("m_ScaleX");
            this.m_ScaleY = this.serializedObject.FindProperty("m_ScaleY");
            this.m_AccuracyFill = this.serializedObject.FindProperty("m_AccuracyFill");
            this.m_Fill = this.serializedObject.FindProperty("m_Fill");
            this.m_AccuracyAlpha = this.serializedObject.FindProperty("m_AccuracyAlpha");
            this.m_Alpha = this.serializedObject.FindProperty("m_Alpha");

            this.m_FieldReticle = new PropertyField(this.m_Reticle);
            this.m_FieldDirection = new PropertyField(this.m_Direction);
            this.m_FieldAccuracyPosition = new PropertyField(this.m_AccuracyPosition);
            this.m_FieldPositionX = new CrosshairRangeField(this.m_PositionX);
            this.m_FieldPositionY = new CrosshairRangeField(this.m_PositionY);
            this.m_FieldAccuracySize = new PropertyField(this.m_AccuracySize);
            this.m_FieldWidth = new CrosshairRangeField(this.m_Width);
            this.m_FieldHeight = new CrosshairRangeField(this.m_Height);
            this.m_FieldAccuracyRotation = new PropertyField(this.m_AccuracyRotation);
            this.m_FieldRotation = new CrosshairRangeField(this.m_Rotation);
            this.m_FieldAccuracyScale = new PropertyField(this.m_AccuracyScale);
            this.m_FieldScaleX = new CrosshairRangeField(this.m_ScaleX);
            this.m_FieldScaleY = new CrosshairRangeField(this.m_ScaleY);
            this.m_FieldAccuracyFill = new PropertyField(this.m_AccuracyFill);
            this.m_FieldFill = new CrosshairRangeField(this.m_Fill);
            this.m_FieldAccuracyAlpha = new PropertyField(this.m_AccuracyAlpha);
            this.m_FieldAlpha = new CrosshairRangeField(this.m_Alpha);
            
            root.Add(this.m_FieldReticle);
            root.Add(this.m_FieldDirection);
            root.Add(new SpaceSmaller());
            root.Add(this.m_FieldAccuracyPosition);
            root.Add(this.m_FieldPositionX);
            root.Add(this.m_FieldPositionY);
            root.Add(new SpaceSmaller());
            root.Add(this.m_FieldAccuracySize);
            root.Add(this.m_FieldWidth);
            root.Add(this.m_FieldHeight);
            root.Add(new SpaceSmaller());
            root.Add(this.m_FieldAccuracyRotation);
            root.Add(this.m_FieldRotation);
            root.Add(new SpaceSmaller());
            root.Add(this.m_FieldAccuracyScale);
            root.Add(this.m_FieldScaleX);
            root.Add(this.m_FieldScaleY);
            root.Add(new SpaceSmaller());
            root.Add(this.m_FieldAccuracyFill);
            root.Add(this.m_FieldFill);
            root.Add(new SpaceSmaller());
            root.Add(this.m_FieldAccuracyAlpha);
            root.Add(this.m_FieldAlpha);
            
            this.m_FieldAccuracyPosition.RegisterValueChangeCallback(this.OnChange);
            this.m_FieldAccuracySize.RegisterValueChangeCallback(this.OnChange);
            this.m_FieldAccuracyRotation.RegisterValueChangeCallback(this.OnChange);
            this.m_FieldAccuracyScale.RegisterValueChangeCallback(this.OnChange);
            this.m_FieldAccuracyFill.RegisterValueChangeCallback(this.OnChange);
            this.m_FieldAccuracyAlpha.RegisterValueChangeCallback(this.OnChange);
            
            this.Refresh();
            
            return root;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnChange(SerializedPropertyChangeEvent changeEvent)
        {
            this.Refresh();
        }
        
        private void Refresh()
        {
            this.m_FieldPositionX.SetEnabled(this.m_AccuracyPosition.objectReferenceValue != null);
            this.m_FieldPositionY.SetEnabled(this.m_AccuracyPosition.objectReferenceValue != null);
            
            this.m_FieldWidth.SetEnabled(this.m_AccuracySize.objectReferenceValue != null);
            this.m_FieldHeight.SetEnabled(this.m_AccuracySize.objectReferenceValue != null);
            
            this.m_FieldRotation.SetEnabled(this.m_AccuracyRotation.objectReferenceValue != null);
            
            this.m_FieldScaleX.SetEnabled(this.m_AccuracyScale.objectReferenceValue != null);
            this.m_FieldScaleY.SetEnabled(this.m_AccuracyScale.objectReferenceValue != null);
            
            this.m_FieldFill.SetEnabled(this.m_AccuracyFill.objectReferenceValue != null);
            this.m_FieldAlpha.SetEnabled(this.m_AccuracyAlpha.objectReferenceValue != null);
        }
    }
}