using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomEditor(typeof(Equipment))]
    public class EquipmentEditor : UnityEditor.Editor
    {
        private const string PROP_EQUIPMENT_SLOTS = "m_Slots";
        
        // MEMBERS: -------------------------------------------------------------------------------

        private VisualElement m_Root;
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            this.m_Root.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new LabelTitle("Equipment"));

            SerializedProperty slots = this.serializedObject.FindProperty("m_Slots"); 
            this.m_Root.Add(new EquipmentTool(slots, PROP_EQUIPMENT_SLOTS));

            return this.m_Root;
        }
    }
}