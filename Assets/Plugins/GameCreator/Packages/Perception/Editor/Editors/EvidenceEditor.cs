using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomEditor(typeof(Evidence))]
    public class EvidenceEditor : UnityEditor.Editor
    {
        private VisualElement m_Root;
        
        private VisualElement m_ContentSensorSee;
        private VisualElement m_ContentSensorHear;
        private VisualElement m_ContentSensorSmell;

        private SerializedProperty m_SensorSee;
        private SerializedProperty m_SensorHear;
        private SerializedProperty m_SensorSmell;
        
        private TextField m_IsTampered;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
            
            Evidence evidence = this.target as Evidence;
            if (evidence == null) return;

            evidence.EventChange -= this.RefreshIsTampered;
            evidence.EventChange += this.RefreshIsTampered;
        }

        private void OnDisable()
        {
            Evidence evidence = this.target as Evidence;
            if (evidence == null) return;

            evidence.EventChange -= this.RefreshIsTampered;
        }

        // INSPECTOR METHODS: ---------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            SerializedProperty tag = this.serializedObject.FindProperty("m_Tag");
            this.m_Root.Add(new PropertyField(tag));

            this.m_SensorSee = this.serializedObject.FindProperty("m_OnSight");
            this.m_SensorHear = this.serializedObject.FindProperty("m_EmitNoise");
            this.m_SensorSmell = this.serializedObject.FindProperty("m_EmitScent");

            ContentBox boxSee = new ContentBox("See", false);
            ContentBox boxHear = new ContentBox("Hear", false);
            ContentBox boxSmell = new ContentBox("Smell", false);
            
            this.m_Root.Add(new SpaceSmaller());
            this.m_Root.Add(boxSee);
            this.m_Root.Add(boxHear);
            this.m_Root.Add(boxSmell);
            
            PropertyField fieldSensorSee = new PropertyField(this.m_SensorSee);
            PropertyField fieldSensorHear = new PropertyField(this.m_SensorHear);
            PropertyField fieldSensorSmell = new PropertyField(this.m_SensorSmell);

            this.m_ContentSensorSee = new VisualElement { style = { marginLeft = new Length(10, LengthUnit.Pixel) }};
            this.m_ContentSensorHear = new VisualElement { style = { marginLeft = new Length(10, LengthUnit.Pixel) }};
            this.m_ContentSensorSmell = new VisualElement { style = { marginLeft = new Length(10, LengthUnit.Pixel) }};
            
            boxSee.Content.Add(new SpaceSmallest());
            boxSee.Content.Add(fieldSensorSee);
            boxSee.Content.Add(this.m_ContentSensorSee);
            
            boxHear.Content.Add(new SpaceSmallest());
            boxHear.Content.Add(fieldSensorHear);
            boxHear.Content.Add(this.m_ContentSensorHear);
            
            boxSmell.Content.Add(new SpaceSmallest());
            boxSmell.Content.Add(fieldSensorSmell);
            boxSmell.Content.Add(this.m_ContentSensorSmell);
            
            SerializedProperty noiseRadius = this.serializedObject.FindProperty("m_NoiseRadius");
            SerializedProperty noiseLevel = this.serializedObject.FindProperty("m_NoiseLevel");
            
            this.m_ContentSensorHear.Add(new PropertyField(noiseRadius, "Radius"));
            this.m_ContentSensorHear.Add(new PropertyField(noiseLevel, "Level"));
            
            SerializedProperty scentInterval = this.serializedObject.FindProperty("m_ScentInterval");
            SerializedProperty scentRadius = this.serializedObject.FindProperty("m_ScentRadius");
            SerializedProperty scentLevel = this.serializedObject.FindProperty("m_ScentLevel");
            
            this.m_ContentSensorSmell.Add(new PropertyField(scentInterval, "Interval"));
            this.m_ContentSensorSmell.Add(new PropertyField(scentRadius, "Radius"));
            this.m_ContentSensorSmell.Add(new PropertyField(scentLevel, "Level"));

            fieldSensorSee.RegisterValueChangeCallback(_ => this.RefreshContentSee());
            fieldSensorHear.RegisterValueChangeCallback(_ => this.RefreshContentHear());
            fieldSensorSmell.RegisterValueChangeCallback(_ => this.RefreshContentSmell());
            
            this.RefreshContentSee();
            this.RefreshContentHear();
            this.RefreshContentSmell();
            
            SerializedProperty startTampered = this.serializedObject.FindProperty("m_StartTampered");
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(startTampered));
            
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                this.m_IsTampered = new TextField("Is Tampered");
                this.m_IsTampered.AddToClassList(AlignLabel.CLASS_UNITY_ALIGN_LABEL);
                this.m_IsTampered.SetEnabled(false);
                
                this.m_Root.Add(this.m_IsTampered);
                this.RefreshIsTampered();
            }
            
            return this.m_Root;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshContentSee()
        {
            this.m_ContentSensorSee.style.display = this.m_SensorSee.boolValue
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }
        
        private void RefreshContentHear()
        {
            this.m_ContentSensorHear.style.display = this.m_SensorHear.boolValue
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }
        
        private void RefreshContentSmell()
        {
            this.m_ContentSensorSmell.style.display = this.m_SensorSmell.boolValue
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }
        
        private void RefreshIsTampered()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (this.m_IsTampered == null) return;

            Evidence evidence = this.target as Evidence;
            if (evidence != null) this.m_IsTampered.value = evidence.IsTampered 
                ? "True"
                : "False";
        }
    }   
}
