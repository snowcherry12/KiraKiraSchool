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
    [CustomEditor(typeof(PriceUI))]
    public class PriceUIEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            SerializedProperty prefabCoin = this.serializedObject.FindProperty("m_PrefabCoin");
            SerializedProperty coinsContent = this.serializedObject.FindProperty("m_CoinsContent");

            PropertyField fieldTitle = new PropertyField(prefabCoin);
            PropertyField fieldAmount = new PropertyField(coinsContent);

            this.m_Root.Add(fieldTitle);
            this.m_Root.Add(fieldAmount);

            return this.m_Root;
        }
        
        // CREATE: --------------------------------------------------------------------------------

        [MenuItem("GameObject/Game Creator/UI/Inventory/Bag/Price UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "PriceUI";
            gameObject.AddComponent<PriceUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}