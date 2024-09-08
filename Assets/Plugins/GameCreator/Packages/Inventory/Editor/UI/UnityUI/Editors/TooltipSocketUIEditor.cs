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
    [CustomEditor(typeof(TooltipSocketUI))]
    public class TooltipSocketUIEditor : TTooltipUIEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = base.CreateInspectorGUI();
            
            SerializedProperty itemInfo = this.serializedObject.FindProperty("m_ItemUI");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(itemInfo));
            
            return root;
        }
        
        // CREATE: --------------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/UI/Inventory/Tooltips/Tooltip Socket UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "TooltipSocketUI";
            gameObject.AddComponent<TooltipSocketUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}