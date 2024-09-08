using GameCreator.Editor.Common.UnityUI;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Editor.Inventory.UnityUI
{
    [CustomEditor(typeof(BagGridUI))]
    public class BagGridUIEditor : TBagUIEditor
    {
        private SerializedProperty m_Content;
        
        protected override void CreateSpecificInspectorGUI()
        {
            this.m_Content = this.serializedObject.FindProperty("m_Content");
            this.m_Root.Add(new PropertyField(this.m_Content));
        }
        
        // CREATE: --------------------------------------------------------------------------------

        [MenuItem("GameObject/Game Creator/UI/Inventory/Bag Grid UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "BagGridUI";
            gameObject.AddComponent<BagGridUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}