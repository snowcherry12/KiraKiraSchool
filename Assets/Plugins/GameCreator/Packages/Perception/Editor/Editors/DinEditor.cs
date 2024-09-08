using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomEditor(typeof(Din))]
    public class DinEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        // INSPECTOR METHODS: ---------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            SerializedProperty radius = this.serializedObject.FindProperty("m_Radius");
            SerializedProperty rollOff = this.serializedObject.FindProperty("m_RollOff");
            SerializedProperty useObstruction = this.serializedObject.FindProperty("m_UseObstruction");
            SerializedProperty level = this.serializedObject.FindProperty("m_Level");
            
            this.m_Root.Add(new SpaceSmaller());
            this.m_Root.Add(new PropertyField(radius));
            this.m_Root.Add(new PropertyField(rollOff));
            this.m_Root.Add(new PropertyField(useObstruction));
            this.m_Root.Add(new SpaceSmaller());
            this.m_Root.Add(new PropertyField(level));
            
            SerializedProperty audioSource = this.serializedObject.FindProperty("m_AudioSource");
            SerializedProperty volume = this.serializedObject.FindProperty("m_Volume");

            ContentBox box = new ContentBox("Audio Source", true);
            box.Content.Add(new PropertyField(audioSource));
            box.Content.Add(new SpaceSmaller());
            box.Content.Add(new PropertyField(volume));
            
            this.m_Root.Add(new SpaceSmaller());
            this.m_Root.Add(box);
            
            return this.m_Root;
        }
    }   
}
