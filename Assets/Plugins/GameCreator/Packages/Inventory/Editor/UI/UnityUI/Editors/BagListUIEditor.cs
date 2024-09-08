using GameCreator.Editor.Common.UnityUI;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Editor.Inventory.UnityUI
{
    [CustomEditor(typeof(BagListUI))]
    public class BagListUIEditor : TBagUIEditor
    {
        private SerializedProperty m_HideEquipped;
        private SerializedProperty m_FilterByParent;
        private SerializedProperty m_Content;
        
        protected override void CreateSpecificInspectorGUI()
        {
            this.m_FilterByParent = this.serializedObject.FindProperty("m_FilterByParent");
            this.m_Content = this.serializedObject.FindProperty("m_Content");
            this.m_HideEquipped = this.serializedObject.FindProperty("m_HideEquipped");
            
            this.m_Root.Add(new PropertyField(this.m_FilterByParent));
            this.m_Root.Add(new PropertyField(this.m_Content));
            this.m_Root.Add(new PropertyField(this.m_HideEquipped));
        }
        
        // CREATE: --------------------------------------------------------------------------------

        [MenuItem("GameObject/Game Creator/UI/Inventory/Bag List UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "BagListUI";
            gameObject.AddComponent<BagListUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}