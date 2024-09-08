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
    [CustomEditor(typeof(BagEquipUI))]
    public class BagEquipUIEditor : UnityEditor.Editor
    {
        private const string ERR_NO_GRAPHIC = "No Graphic component to receive raycast events";
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_Root;

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            SerializedProperty bagUI = this.serializedObject.FindProperty("m_Bag");
            SerializedProperty index = this.serializedObject.FindProperty("m_EquipmentIndex");
            
            SerializedProperty itemBaseUI = this.serializedObject.FindProperty("m_BaseUI");
            SerializedProperty itemEquipUI = this.serializedObject.FindProperty("m_EquippedUI");
            
            SerializedProperty onClick = this.serializedObject.FindProperty("m_OnClick");
            SerializedProperty onSubmit = this.serializedObject.FindProperty("m_OnSubmit");
            SerializedProperty onDrop = this.serializedObject.FindProperty("m_OnDrop");
            
            SerializedProperty input = this.serializedObject.FindProperty("m_InputUse");
            
            SerializedProperty activeNotCooldown = this.serializedObject.FindProperty("m_ActiveNotCooldown");
            SerializedProperty cooldownProgress = this.serializedObject.FindProperty("m_CooldownProgress");

            BagEquipUI bagEquipUI = this.target as BagEquipUI;
            if (bagEquipUI != null)
            {
                Graphic graphic = bagEquipUI.GetComponent<Graphic>();
                if (graphic == null) this.m_Root.Add(new ErrorMessage(ERR_NO_GRAPHIC));
            }

            this.m_Root.Add(new PropertyField(bagUI));
            this.m_Root.Add(new PropertyField(index));
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(itemBaseUI));
            this.m_Root.Add(new PropertyField(itemEquipUI));
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(onSubmit));
            this.m_Root.Add(new PropertyField(onClick));
            this.m_Root.Add(new PropertyField(onDrop));
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(input));
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(activeNotCooldown));
            this.m_Root.Add(new PropertyField(cooldownProgress));
            
            return this.m_Root;
        }
        
        // CREATE: --------------------------------------------------------------------------------

        [MenuItem("GameObject/Game Creator/UI/Inventory/Bag/Bag Equip UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            
            gameObject.name = "BagEquipUI";
            gameObject.AddComponent<BagEquipUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}