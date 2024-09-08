using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory.UnityUI
{
    [CustomEditor(typeof(TBagUI))]
    public abstract class TBagUIEditor : UnityEditor.Editor
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        protected VisualElement m_Root;
        
        private VisualElement m_ContentDropOutside;
        
        private SerializedProperty m_DefaultBag;
        private SerializedProperty m_PrefabCell;
        private SerializedProperty m_CanDropOutside;
        private SerializedProperty m_MaxDropDistance;

        private PropertyField m_FieldCanDropOutside;
        private PropertyField m_FieldMaxDropDistance;

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            this.m_ContentDropOutside = new VisualElement();

            this.m_DefaultBag = this.serializedObject.FindProperty("m_DefaultBag");
            this.m_Root.Add(new PropertyField(this.m_DefaultBag));
            this.m_Root.Add(new SpaceSmall());
            
            this.m_PrefabCell = this.serializedObject.FindProperty("m_PrefabCell");
            this.m_Root.Add(new PropertyField(this.m_PrefabCell));
            
            this.CreateSpecificInspectorGUI();
            
            this.m_Root.Add(new SpaceSmall());
            this.CreateDropOutsideContent();

            return this.m_Root;
        }

        private void CreateDropOutsideContent()
        {
            this.m_CanDropOutside = this.serializedObject.FindProperty("m_CanDropOutside");
            this.m_MaxDropDistance = this.serializedObject.FindProperty("m_MaxDropDistance");

            this.m_FieldCanDropOutside = new PropertyField(this.m_CanDropOutside);
            this.m_FieldMaxDropDistance = new PropertyField(this.m_MaxDropDistance);

            this.m_Root.Add(this.m_FieldCanDropOutside);
            this.m_Root.Add(this.m_ContentDropOutside);
            
            this.RefreshDropOutsideContent();
            this.m_FieldCanDropOutside.RegisterValueChangeCallback(_ =>
            {
                this.RefreshDropOutsideContent();
            });
        }

        // REFRESH METHODS: -----------------------------------------------------------------------
        
        private void RefreshDropOutsideContent()
        {
            this.m_ContentDropOutside.Clear();
            if (!this.m_CanDropOutside.boolValue) return;
            
            this.m_FieldMaxDropDistance.Bind(this.serializedObject);
            this.m_ContentDropOutside.Add(this.m_FieldMaxDropDistance);
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        protected abstract void CreateSpecificInspectorGUI();
    }
}