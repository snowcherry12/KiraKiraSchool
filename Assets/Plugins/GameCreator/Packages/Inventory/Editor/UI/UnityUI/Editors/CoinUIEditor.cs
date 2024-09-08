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
    [CustomEditor(typeof(CoinUI))]
    public class CoinUIEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            SerializedProperty title = this.serializedObject.FindProperty("m_CoinName");
            SerializedProperty amount = this.serializedObject.FindProperty("m_CoinAmount");
            SerializedProperty color = this.serializedObject.FindProperty("m_CoinColor");
            SerializedProperty image = this.serializedObject.FindProperty("m_CoinImage");

            PropertyField fieldTitle = new PropertyField(title);
            PropertyField fieldAmount = new PropertyField(amount);
            PropertyField fieldColor = new PropertyField(color);
            PropertyField fieldImage = new PropertyField(image);
            
            this.m_Root.Add(fieldTitle);
            this.m_Root.Add(fieldAmount);
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(fieldColor);
            this.m_Root.Add(fieldImage);
            
            return this.m_Root;
        }
        
        // CREATE: --------------------------------------------------------------------------------

        [MenuItem("GameObject/Game Creator/UI/Inventory/Bag/Coin UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "CoinUI";
            gameObject.AddComponent<CoinUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}