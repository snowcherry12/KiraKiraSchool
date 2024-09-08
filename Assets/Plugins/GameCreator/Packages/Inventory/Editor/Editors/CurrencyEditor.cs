using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomEditor(typeof(Currency))]
    public class CurrencyEditor : UnityEditor.Editor
    {
        public static Currency DEFAULT_INSTANCE; 
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_Root;
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            DEFAULT_INSTANCE = this.target as Currency;
            this.m_Root = new VisualElement();

            SerializedProperty coins = this.serializedObject.FindProperty("m_Coins");
            SerializedProperty uniqueID = this.serializedObject.FindProperty("m_UniqueID");
            
            PropertyField fieldCoins = new PropertyField(coins);
            PropertyField fieldUniqueID = new PropertyField(uniqueID);
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(fieldCoins);
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(fieldUniqueID);
            
            return this.m_Root;
        }
        
        // DEFAULT INSTANCE METHODS: --------------------------------------------------------------

        [InitializeOnLoadMethod]
        private static void InitOnLoad()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(Currency)}");
            if (guids.Length == 0) return;

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            Currency currency = AssetDatabase.LoadAssetAtPath<Currency>(path);
            if (currency != null) DEFAULT_INSTANCE = currency;
        }
    }
}