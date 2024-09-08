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
    [CustomEditor(typeof(BagCellUI), true)]
    public class BagCellUIEditor : UnityEditor.Editor
    {
        private const string MSG = "This component is configured by its 'Bag UI' parent component";
        private const string ERR_NO_GRAPHIC = "No Graphic component to receive raycast events";

        private const string PROP_ON_CHOOSE = "m_OnChoose";
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_Root;
        private VisualElement m_ContentChoose;
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            SerializedProperty translucentDrag = this.serializedObject.FindProperty("m_TranslucentDrag");
            SerializedProperty activeIsDrag = this.serializedObject.FindProperty("m_ActiveIsDrag");
            SerializedProperty activeCanDrop = this.serializedObject.FindProperty("m_ActiveCanDrop");
            SerializedProperty activeCannotDrop = this.serializedObject.FindProperty("m_ActiveCannotDrop");
            
            this.m_Root.Add(new InfoMessage(MSG));
            this.m_Root.Add(new PropertyField(translucentDrag));
            this.m_Root.Add(new PropertyField(activeIsDrag));
            
            this.m_Root.Add(new SpaceSmaller());
            this.m_Root.Add(new PropertyField(activeCanDrop));
            this.m_Root.Add(new PropertyField(activeCannotDrop));
            this.m_Root.Add(new SpaceSmaller());
            
            SerializedProperty cellInfo = this.serializedObject.FindProperty("m_CellInfo");
            SerializedProperty cellMerchant = this.serializedObject.FindProperty("m_CellMerchant");
            SerializedProperty canDrag = this.serializedObject.FindProperty("m_CanDrag");
            SerializedProperty activeSelection = this.serializedObject.FindProperty("m_ActiveSelection");
            SerializedProperty onDrop = this.serializedObject.FindProperty("m_OnDrop");
            SerializedProperty onChoose = this.serializedObject.FindProperty(PROP_ON_CHOOSE);
            
            BagCellUI bagCellUI = this.target as BagCellUI;
            if (bagCellUI != null)
            {
                Graphic graphic = bagCellUI.GetComponent<Graphic>();
                if (graphic == null) this.m_Root.Add(new ErrorMessage(ERR_NO_GRAPHIC));
            }

            this.m_ContentChoose = new VisualElement();
            var fieldChoose = new PropertyField(onChoose);
            
            this.m_Root.Add(new PropertyField(cellInfo));
            this.m_Root.Add(new PropertyField(cellMerchant));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(canDrag));
            this.m_Root.Add(new PropertyField(activeSelection));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(onDrop));
            this.m_Root.Add(fieldChoose);
            this.m_Root.Add(this.m_ContentChoose);

            this.RefreshContentChoose();
            fieldChoose.RegisterValueChangeCallback(_ =>
            {
                this.RefreshContentChoose();
            });

            return this.m_Root;
        }

        private void RefreshContentChoose()
        {
            this.serializedObject.Update();
            
            this.m_ContentChoose.Clear();
            SerializedProperty onChoose = this.serializedObject.FindProperty(PROP_ON_CHOOSE);

            if (onChoose.intValue == 3) // Dismantle
            {
                SerializedProperty chance = this.serializedObject.FindProperty("m_DismantleChance");
                PropertyField fieldChance = new PropertyField(chance);
                
                fieldChance.Bind(this.serializedObject);
                this.m_ContentChoose.Add(fieldChance);
            }

            if (onChoose.intValue == 4) // SendToBag
            {
                SerializedProperty allowSendEquipment = this.serializedObject.FindProperty("m_AllowSendEquipment");
                SerializedProperty sendToBag = this.serializedObject.FindProperty("m_SendToBag");

                PropertyField fieldAllowSendEquipment = new PropertyField(allowSendEquipment);
                PropertyField fieldSendToBag = new PropertyField(sendToBag);
                
                fieldAllowSendEquipment.Bind(this.serializedObject);
                fieldSendToBag.Bind(this.serializedObject);
                
                this.m_ContentChoose.Add(fieldAllowSendEquipment);
                this.m_ContentChoose.Add(fieldSendToBag);
            }
        }
        
        // CREATE: --------------------------------------------------------------------------------

        [MenuItem("GameObject/Game Creator/UI/Inventory/Bag/Bag Cell UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "BagCellUI";
            gameObject.AddComponent<BagCellUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}