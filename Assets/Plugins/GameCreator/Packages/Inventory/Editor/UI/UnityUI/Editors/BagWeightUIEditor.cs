using System;
using GameCreator.Editor.Common;
using GameCreator.Editor.Common.UnityUI;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory.UnityUI
{
    [CustomEditor(typeof(BagWeightUI))]
    public class BagWeightUIEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        private SerializedProperty m_FromBag;
        private SerializedProperty m_WeightCurrent;
        private SerializedProperty m_WeightMax;
        
        private PropertyField m_FieldFromBag;
        private PropertyField m_FieldWeightCurrent;
        private PropertyField m_FieldWeightMax;

        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            this.m_FromBag = this.serializedObject.FindProperty("m_FromBag");
            this.m_WeightCurrent = this.serializedObject.FindProperty("m_WeightCurrent");
            this.m_WeightMax = this.serializedObject.FindProperty("m_WeightMax");

            this.m_FieldFromBag = new PropertyField(this.m_FromBag);
            this.m_FieldWeightCurrent = new PropertyField(this.m_WeightCurrent);
            this.m_FieldWeightMax = new PropertyField(this.m_WeightMax);

            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(this.m_FieldFromBag);

            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(this.m_FieldWeightCurrent);
            this.m_Root.Add(this.m_FieldWeightMax);

            return this.m_Root;
        }
        
        // CREATE: --------------------------------------------------------------------------------

        [MenuItem("GameObject/Game Creator/UI/Inventory/Bag/Bag Weight UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "BagWeightUI";
            gameObject.AddComponent<BagWeightUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}