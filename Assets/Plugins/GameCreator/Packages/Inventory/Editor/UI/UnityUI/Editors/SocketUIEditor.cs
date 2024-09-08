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
    [CustomEditor(typeof(SocketUI))]
    public class SocketUIEditor : UnityEditor.Editor
    {
        private const string MSG = "This component is automatically configured by its parent";
        private const string ERR_NO_GRAPHIC = "No Graphic component to receive raycast events";
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_Root;
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            SerializedProperty itemUI = this.serializedObject.FindProperty("m_ItemUI");
            
            SerializedProperty onDrop = this.serializedObject.FindProperty("m_OnDrop");
            SerializedProperty onSubmit = this.serializedObject.FindProperty("m_OnSubmit");

            this.m_Root.Add(new InfoMessage(MSG));
            
            SocketUI socketUI = this.target as SocketUI;
            if (socketUI != null)
            {
                Graphic graphic = socketUI.GetComponent<Graphic>();
                if (graphic == null) this.m_Root.Add(new ErrorMessage(ERR_NO_GRAPHIC));
            }
            
            this.m_Root.Add(new PropertyField(itemUI));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(onDrop));
            this.m_Root.Add(new PropertyField(onSubmit));
            
            return this.m_Root;
        }
        
        // CREATE: --------------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/UI/Inventory/Item/Socket UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "SocketUI";
            gameObject.AddComponent<SocketUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}