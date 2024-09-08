using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using UnityEditor;
using GameCreator.Runtime.Inventory;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomEditor(typeof(Bag))]
    public class BagEditor : UnityEditor.Editor
    {
        private const string USS_PATH = EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Bag";
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_Root;
        private VisualElement m_Content;
        
        // PAINT METHODS: -------------------------------------------------------------------------

        private void OnEnable()
        {
            Bag bag = this.target as Bag;
            if (EditorApplication.isPlayingOrWillChangePlaymode && bag != null)
            {
                bag.EventChange -= this.Paint;
                bag.EventChange += this.Paint;
            }
        }

        private void OnDisable()
        {
            Bag bag = this.target as Bag;
            if (bag != null) bag.EventChange += this.Paint;
        }

        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            this.m_Content = new VisualElement();

            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets) this.m_Root.styleSheets.Add(sheet);

            this.Paint();

            SerializedProperty stock = this.serializedObject.FindProperty("m_Stock");
            SerializedProperty skinUI = this.serializedObject.FindProperty("m_SkinUI");
            SerializedProperty wearer = this.serializedObject.FindProperty("m_Wearer");

            PropertyField fieldStock = new PropertyField(stock);
            PropertyField fieldSkinUI = new PropertyField(skinUI);
            PropertyField fieldWearer = new PropertyField(wearer);

            this.m_Root.Add(this.m_Content);
            
            this.m_Root.Add(new LabelTitle("Stock"));
            this.m_Root.Add(new SpaceSmaller());
            this.m_Root.Add(fieldStock);
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(fieldSkinUI);
            this.m_Root.Add(fieldWearer);

            return this.m_Root;
        }

        private void Paint()
        {
            if (this.m_Content == null) return;
            this.m_Content.Clear();
            
            SerializedProperty bag = this.serializedObject.FindProperty("m_Bag");
            PropertyField fieldBag = new PropertyField(bag);
            
            this.m_Content.Add(fieldBag);
            fieldBag.Bind(this.serializedObject);
        }
    }
}
