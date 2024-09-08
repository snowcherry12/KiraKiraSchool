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
    [CustomEditor(typeof(DismantlingItemUI))]
    public class DismantlingItemUIEditor : TTinkerItemUIEditor
    {
        private VisualElement m_Root;

        protected override string LabelButtonTinker => "Button Dismantle";
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = base.CreateInspectorGUI();

            SerializedProperty recoverChance = this.serializedObject.FindProperty("m_RecoverChance");
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(recoverChance));
            
            return this.m_Root;
        }
        
        // CREATE: --------------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/UI/Inventory/Dismantling/Item UI", false, 0)]
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