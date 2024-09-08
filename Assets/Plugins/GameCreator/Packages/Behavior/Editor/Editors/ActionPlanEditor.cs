using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomEditor(typeof(ActionPlan))]
    public class ActionPlanEditor : GraphEditor
    {
        protected override string TypeName => "Action Plan";
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected override void OpenGraph()
        {
            ActionPlan instance = this.target as ActionPlan;
            OpenAsset(instance);
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = base.CreateInspectorGUI();

            SerializedProperty thoughts = this.serializedObject.FindProperty("m_Thoughts");
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("Initial Beliefs:"));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(thoughts));
            
            return root;
        }

        // OPEN ASSET: ----------------------------------------------------------------------------
        
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            ActionPlan instance = EditorUtility.InstanceIDToObject(instanceID) as ActionPlan;
            if (instance == null) return false;
            
            OpenAsset(instance);
            return true;
        }

        private static void OpenAsset(ActionPlan actionPlan)
        {
            WindowActionPlan.Open(actionPlan);
        }
    }
}