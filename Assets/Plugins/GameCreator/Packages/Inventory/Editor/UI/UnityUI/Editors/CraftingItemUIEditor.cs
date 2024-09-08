using GameCreator.Editor.Common.UnityUI;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory.UnityUI
{
    [CustomEditor(typeof(CraftingItemUI))]
    public class CraftingItemUIEditor : TTinkerItemUIEditor
    {
        private VisualElement m_Root;

        protected override string LabelButtonTinker => "Button Craft";

        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = base.CreateInspectorGUI();
            
            return this.m_Root;
        }
        
        // CREATE: --------------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/UI/Inventory/Crafting/Item UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "CraftingItemUI";
            gameObject.AddComponent<CraftingItemUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}