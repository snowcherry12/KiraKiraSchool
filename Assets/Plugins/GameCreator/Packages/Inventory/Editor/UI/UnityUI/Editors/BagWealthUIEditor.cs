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
    [CustomEditor(typeof(BagWealthUI))]
    public class BagWealthUIEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        private SerializedProperty m_Currency;
        private SerializedProperty m_FromBag;
        private SerializedProperty m_PrefabCoin;
        private SerializedProperty m_CoinsContent;
        
        private PropertyField m_FieldCurrency;
        private PropertyField m_FieldFromBag;
        private PropertyField m_FieldPrefabCoin;
        private PropertyField m_FieldCoinsContent;

        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            this.m_Currency = this.serializedObject.FindProperty("m_Currency");
            this.m_FromBag = this.serializedObject.FindProperty("m_FromBag");
            this.m_PrefabCoin = this.serializedObject.FindProperty("m_PrefabCoin");
            this.m_CoinsContent = this.serializedObject.FindProperty("m_CoinsContent");

            this.m_FieldCurrency = new PropertyField(this.m_Currency);
            this.m_FieldFromBag = new PropertyField(this.m_FromBag);
            this.m_FieldPrefabCoin = new PropertyField(this.m_PrefabCoin);
            this.m_FieldCoinsContent = new PropertyField(this.m_CoinsContent);

            this.m_Root.Add(this.m_FieldCurrency);
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(this.m_FieldFromBag);

            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(this.m_FieldPrefabCoin);
            this.m_Root.Add(this.m_FieldCoinsContent);

            return this.m_Root;
        }
        
        // CREATE: --------------------------------------------------------------------------------

        [MenuItem("GameObject/Game Creator/UI/Inventory/Bag/Bag Wealth UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "BagWealthUI";
            gameObject.AddComponent<BagWealthUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}