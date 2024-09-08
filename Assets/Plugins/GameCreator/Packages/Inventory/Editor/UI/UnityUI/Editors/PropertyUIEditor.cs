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
    [CustomEditor(typeof(PropertyUI))]
    public class PropertyUIEditor : UnityEditor.Editor
    {
        private const string MSG = "This component is automatically configured by its parent";
        private const string ERR_NO_GRAPHIC = "No Graphic component to receive raycast events";
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_Root;

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            SerializedProperty numberValue = this.serializedObject.FindProperty("m_NumberValue");
            SerializedProperty stringValue = this.serializedObject.FindProperty("m_StringValue");
            SerializedProperty icon = this.serializedObject.FindProperty("m_Icon");
            SerializedProperty color = this.serializedObject.FindProperty("m_Color");
            
            this.m_Root.Add(new InfoMessage(MSG));
            PropertyUI propertyUI = this.target as PropertyUI;
            if (propertyUI != null)
            {
                Graphic graphic = propertyUI.GetComponent<Graphic>();
                if (graphic == null) this.m_Root.Add(new ErrorMessage(ERR_NO_GRAPHIC));
            }
            
            this.m_Root.Add(new PropertyField(numberValue));
            this.m_Root.Add(new PropertyField(stringValue));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(icon));
            this.m_Root.Add(new PropertyField(color));
            
            return this.m_Root;
        }
        
        // CREATE: --------------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/UI/Inventory/Item/Property UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "PropertyUI";
            gameObject.AddComponent<PropertyUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}