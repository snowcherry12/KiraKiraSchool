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
    [CustomEditor(typeof(TooltipDragUI))]
    public class TooltipDragUIEditor : TTooltipUIEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = base.CreateInspectorGUI();

            SerializedProperty itemUI = this.serializedObject.FindProperty("m_ItemUI");
            SerializedProperty cursorIcon = this.serializedObject.FindProperty("m_CursorDragIcon");
            SerializedProperty cursorPoint = this.serializedObject.FindProperty("m_CursorDragPointer");

            root.Add(new SpaceSmall());
            root.Add(new PropertyField(itemUI));
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(cursorIcon));
            root.Add(new PropertyField(cursorPoint));

            return root;
        }
        
        // CREATE: --------------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/UI/Inventory/Tooltips/Tooltip Drag UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "TooltipDragUI";
            gameObject.AddComponent<TooltipDragUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}