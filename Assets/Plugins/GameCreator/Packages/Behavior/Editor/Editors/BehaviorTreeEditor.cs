using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.Callbacks;

namespace GameCreator.Editor.Behavior
{
    [CustomEditor(typeof(BehaviorTree))]
    public class BehaviorTreeEditor : GraphEditor
    {
        protected override string TypeName => "Behavior Tree";
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected override void OpenGraph()
        {
            BehaviorTree instance = this.target as BehaviorTree;
            OpenAsset(instance);
        }
        
        // OPEN ASSET: ----------------------------------------------------------------------------
        
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            BehaviorTree instance = EditorUtility.InstanceIDToObject(instanceID) as BehaviorTree;
            if (instance == null) return false;
            
            OpenAsset(instance);
            return true;
        }

        private static void OpenAsset(BehaviorTree behaviorTree)
        {
            WindowBehaviorTree.Open(behaviorTree);
        }
    }
}