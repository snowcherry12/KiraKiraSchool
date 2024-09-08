using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomEditor(typeof(Obstruction))]
    public class ObstructionEditor : UnityEditor.Editor
    {
        private const string ERR_NO_COLLIDER = "This component requires a Collider";
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private VisualElement m_Root;
        
        // INSPECTOR METHODS: ---------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            Obstruction obstruction = this.target as Obstruction;
            if (obstruction != null && !obstruction.GetComponent<Collider>())
            {
                this.m_Root.Add(new SpaceSmaller());
                this.m_Root.Add(new ErrorMessage(ERR_NO_COLLIDER));
            }

            SerializedProperty noiseDamping = this.serializedObject.FindProperty("m_NoiseDamping");
            SerializedProperty sightDamping = this.serializedObject.FindProperty("m_SightDamping");
            
            this.m_Root.Add(new SpaceSmaller());
            this.m_Root.Add(new PropertyField(noiseDamping));
            this.m_Root.Add(new SpaceSmaller());
            this.m_Root.Add(new PropertyField(sightDamping));
            
            return this.m_Root;
        }
    }   
}
