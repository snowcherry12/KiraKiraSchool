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
    [CustomEditor(typeof(DismantlingIngredientUI))]
    public class DismantlingIngredientUIEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            SerializedProperty ingredientUI = this.serializedObject.FindProperty("m_IngredientUI");
            SerializedProperty amountToRetrieve = this.serializedObject.FindProperty("m_AmountToRetrieve");
            SerializedProperty amountInBag = this.serializedObject.FindProperty("m_AmountInBag");
            
            this.m_Root.Add(new PropertyField(ingredientUI));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(amountToRetrieve));
            this.m_Root.Add(new PropertyField(amountInBag));

            return this.m_Root;
        }
        
        // CREATE: --------------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/UI/Inventory/Dismantling/Ingredient UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "DismantlingItemUI";
            gameObject.AddComponent<DismantlingItemUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}