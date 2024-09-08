using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomEditor(typeof(Merchant))]
    public class MerchantEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;

        private VisualElement m_ContentBuyType;

        private SerializedProperty m_Info;
        
        private SerializedProperty m_InfiniteCurrency;
        private SerializedProperty m_InfiniteStock;
        private SerializedProperty m_AllowBuyBack;
        private SerializedProperty m_SellNicheType;
        private SerializedProperty m_SellType;
        private SerializedProperty m_BuyRate;
        private SerializedProperty m_SellRate;
        private SerializedProperty m_Bag;
        private SerializedProperty m_SkinUI;

        private PropertyField m_FieldInfo;
        private PropertyField m_FieldInfiniteCurrency;
        private PropertyField m_FieldInfiniteStock;
        private PropertyField m_FieldAllowBuyBack;
        private PropertyField m_FieldSellNicheType;
        private PropertyField m_FieldSellType;
        private PropertyField m_FieldBuyRate;
        private PropertyField m_FieldSellRate;
        private PropertyField m_FieldMerchantBag;
        private PropertyField m_FieldMerchantSkin;
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            this.m_Info = this.serializedObject.FindProperty("m_Info");
            this.m_InfiniteCurrency = this.serializedObject.FindProperty("m_InfiniteCurrency");
            this.m_InfiniteStock = this.serializedObject.FindProperty("m_InfiniteStock");
            this.m_AllowBuyBack = this.serializedObject.FindProperty("m_AllowBuyBack");
            this.m_SellNicheType = this.serializedObject.FindProperty("m_SellNicheType");
            this.m_SellType = this.serializedObject.FindProperty("m_SellType");
            this.m_BuyRate = this.serializedObject.FindProperty("m_BuyRate");
            this.m_SellRate = this.serializedObject.FindProperty("m_SellRate");
            this.m_Bag = this.serializedObject.FindProperty("m_Bag");
            this.m_SkinUI = this.serializedObject.FindProperty("m_SkinUI");

            this.m_FieldInfo = new PropertyField(this.m_Info);
            this.m_FieldInfiniteCurrency = new PropertyField(this.m_InfiniteCurrency);
            this.m_FieldInfiniteStock = new PropertyField(this.m_InfiniteStock);
            this.m_FieldAllowBuyBack = new PropertyField(this.m_AllowBuyBack);
            this.m_FieldSellNicheType = new PropertyField(this.m_SellNicheType);
            this.m_FieldSellType = new PropertyField(this.m_SellType);
            this.m_FieldBuyRate = new PropertyField(this.m_BuyRate);
            this.m_FieldSellRate = new PropertyField(this.m_SellRate);
            this.m_FieldMerchantBag = new PropertyField(this.m_Bag);
            this.m_FieldMerchantSkin = new PropertyField(this.m_SkinUI);

            this.m_ContentBuyType = new VisualElement();

            this.m_Root.Add(new SpaceSmaller());
            this.m_Root.Add(this.m_FieldInfo);
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(this.m_FieldInfiniteCurrency);
            this.m_Root.Add(this.m_FieldInfiniteStock);
            this.m_Root.Add(this.m_FieldAllowBuyBack);
            this.m_Root.Add(this.m_FieldSellNicheType);
            this.m_Root.Add(this.m_ContentBuyType);
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(this.m_FieldBuyRate);
            this.m_Root.Add(new SpaceSmaller());
            this.m_Root.Add(this.m_FieldSellRate);
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(this.m_FieldMerchantBag);
            this.m_Root.Add(new SpaceSmaller());
            this.m_Root.Add(this.m_FieldMerchantSkin);

            this.RefreshContentBuyType();
            this.m_FieldSellNicheType.RegisterValueChangeCallback(_ =>
            {
                this.RefreshContentBuyType();
            });
            
            return this.m_Root;
        }

        private void RefreshContentBuyType()
        {
            this.m_ContentBuyType.Clear();
            if (this.m_SellNicheType.boolValue)
            {
                this.m_ContentBuyType.Add(this.m_FieldSellType);
                this.m_FieldSellType.Bind(this.serializedObject);
            }
        }
    }
}