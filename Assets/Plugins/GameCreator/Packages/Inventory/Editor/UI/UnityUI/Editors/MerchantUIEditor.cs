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
    [CustomEditor(typeof(MerchantUI))]
    public class MerchantUIEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            SerializedProperty merchantBagUI = this.serializedObject.FindProperty("m_MerchantBagUI");
            SerializedProperty clientBagUI = this.serializedObject.FindProperty("m_ClientBagUI");
            
            SerializedProperty onBuy = this.serializedObject.FindProperty("m_OnBuy");
            SerializedProperty onSell = this.serializedObject.FindProperty("m_OnSell");

            PropertyField fieldMerchantBagUI = new PropertyField(merchantBagUI);
            PropertyField fieldClientBagUI = new PropertyField(clientBagUI);
            
            this.m_Root.Add(fieldMerchantBagUI);
            this.m_Root.Add(fieldClientBagUI);
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new LabelTitle("On Buy"));
            this.m_Root.Add(new SpaceSmallest());
            this.m_Root.Add(new PropertyField(onBuy));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new LabelTitle("On Sell"));
            this.m_Root.Add(new SpaceSmallest());
            this.m_Root.Add(new PropertyField(onSell));
            
            return this.m_Root;
        }
        
        // CREATE: --------------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/UI/Inventory/Merchant UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "MerchantUI";
            gameObject.AddComponent<MerchantUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}
