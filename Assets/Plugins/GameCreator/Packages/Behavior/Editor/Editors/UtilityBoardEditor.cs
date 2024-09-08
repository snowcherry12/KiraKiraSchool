using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomEditor(typeof(UtilityBoard))]
    public class UtilityBoardEditor : GraphEditor
    {
        protected override string TypeName => "Utility Board";
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected override void OpenGraph()
        {
            UtilityBoard instance = this.target as UtilityBoard;
            OpenAsset(instance);
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = base.CreateInspectorGUI();

            ContentBox limits = new ContentBox("Limits", true);
            
            SerializedProperty minimum = this.serializedObject.FindProperty("m_Minimum");
            SerializedProperty maximum = this.serializedObject.FindProperty("m_Maximum");
            
            root.Add(new SpaceSmall());
            root.Add(limits);
            
            limits.Content.Add(new PropertyField(minimum));
            limits.Content.Add(new SpaceSmaller());
            limits.Content.Add(new PropertyField(maximum));

            return root;
        }

        // OPEN ASSET: ----------------------------------------------------------------------------
        
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            UtilityBoard instance = EditorUtility.InstanceIDToObject(instanceID) as UtilityBoard;
            if (instance == null) return false;
            
            OpenAsset(instance);
            return true;
        }

        private static void OpenAsset(UtilityBoard utilityBoard)
        {
            WindowUtilityBoard.Open(utilityBoard);
        }
    }
}