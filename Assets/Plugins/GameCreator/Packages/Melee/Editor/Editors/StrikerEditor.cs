using GameCreator.Editor.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomEditor(typeof(Striker))]
    public class StrikerEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            SerializedProperty id = this.serializedObject.FindProperty("m_ID");
            SerializedProperty layerMask = this.serializedObject.FindProperty("m_LayerMask");
            SerializedProperty section = this.serializedObject.FindProperty("m_Section");
            SerializedProperty shape = this.serializedObject.FindProperty("m_Shape");

            PropertyField fieldId = new PropertyField(id);
            PropertyField fieldLayerMask = new PropertyField(layerMask);
            PropertyField fieldSection = new PropertyField(section);
            PropertyElement fieldShape = new PropertyElement(shape, shape.displayName, false);
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(fieldId);
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(fieldLayerMask);
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(fieldSection);
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(fieldShape);
            
            SerializedProperty trail = this.serializedObject.FindProperty("m_Trail");
            PropertyField fieldTrail = new PropertyField(trail);
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(fieldTrail);
            
            return this.m_Root;
        }
    }
}