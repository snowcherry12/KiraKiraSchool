using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomEditor(typeof(Combos))]
    public class CombosEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            SerializedProperty combos = this.serializedObject.FindProperty("m_Combos");
            this.m_Root.Add(new PropertyField(combos));
            
            return this.m_Root;
        }

        public override bool UseDefaultMargins() => false;
    }
}