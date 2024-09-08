using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomEditor(typeof(Camouflage))]
    public class CamouflageEditor : UnityEditor.Editor
    {
        // INSPECTOR METHODS: ---------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty sightDamping = this.serializedObject.FindProperty("m_SightDamping");
            
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(sightDamping));

            return root;
        }
    }   
}
