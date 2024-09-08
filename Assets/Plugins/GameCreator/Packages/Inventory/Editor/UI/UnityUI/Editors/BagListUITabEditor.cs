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
    [CustomEditor(typeof(BagListUITab))]
    public class BagListUITabEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            SerializedProperty bagListUI = this.serializedObject.FindProperty("m_BagListUI");
            SerializedProperty filterByParent = this.serializedObject.FindProperty("m_FilterByParent");
            SerializedProperty activeFilter = this.serializedObject.FindProperty("m_ActiveFilter");
            
            this.m_Root.Add(new PropertyField(bagListUI));
            this.m_Root.Add(new PropertyField(filterByParent));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(activeFilter));
            
            return this.m_Root;
        }
        
        // CREATE: --------------------------------------------------------------------------------

        [MenuItem("GameObject/Game Creator/UI/Inventory/Bag/Bag List UI Tab", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "BagListUITab";
            gameObject.AddComponent<BagListUITab>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}
