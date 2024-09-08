using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Luminance))]
    public class LuminanceEditor : UnityEditor.Editor
    {
        private const string MSG_HINT = "Using Light component";
        
        // INSPECTOR METHODS: ---------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            SerializedProperty multiplier = this.serializedObject.FindProperty("m_Multiplier");
            SerializedProperty layerMask = this.serializedObject.FindProperty("m_LayerMask");

            PropertyField fieldMultiplier = new PropertyField(multiplier);
            PropertyField fieldLayerMask = new PropertyField(layerMask);
            
            root.Add(new InfoMessage(MSG_HINT));
            root.Add(fieldMultiplier);
            root.Add(new SpaceSmaller());
            root.Add(fieldLayerMask);

            return root;
        }
    }   
}
