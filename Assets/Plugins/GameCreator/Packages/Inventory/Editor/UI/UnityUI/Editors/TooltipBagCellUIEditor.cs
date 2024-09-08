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
    [CustomEditor(typeof(TooltipBagCellUI))]
    public class TooltipBagCellUIEditor : TTooltipUIEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = base.CreateInspectorGUI();
            
            SerializedProperty itemUI = this.serializedObject.FindProperty("m_ItemUI");
            SerializedProperty cellMerchant = this.serializedObject.FindProperty("m_CellMerchant");
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(itemUI));
            root.Add(new PropertyField(cellMerchant));
            
            return root;
        }
        
        // CREATE: --------------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/UI/Inventory/Tooltips/Tooltip Bag Cell UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "TooltipBagCellUI";
            gameObject.AddComponent<TooltipBagCellUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}