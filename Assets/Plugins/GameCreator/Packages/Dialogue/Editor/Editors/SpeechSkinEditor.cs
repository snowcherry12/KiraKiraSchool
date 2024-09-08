using System.IO;
using System.Reflection;
using GameCreator.Editor.Common;
using GameCreator.Editor.Core;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    [CustomEditor(typeof(SpeechSkin))]
    public class SpeechSkinEditor : SkinEditor
    {
        private const BindingFlags MEMBER_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;
        
        private const string ASSETS = "Assets/";
        
        private const string CONTROLLER_PATH = 
            RuntimePaths.PACKAGES + 
            "Dialogue/Runtime/Assets/Overrides/Speech.overrideController";

        private const string PROP_CONTROLLER = "m_Controller";
        private const string PROP_WHEN = "m_When";
        private const string PROP_IDLE = "m_Idle";
        private const string PROP_OPEN = "m_Open";
        private const string PROP_START = "m_Start";
        private const string PROP_FINISH = "m_Finish";
        private const string PROP_LOG = "m_Log";

        // PAINT METHOD: --------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = base.CreateInspectorGUI();
            
            SerializedProperty when = this.serializedObject.FindProperty(PROP_WHEN);
            SerializedProperty idle = this.serializedObject.FindProperty(PROP_IDLE);
            SerializedProperty open = this.serializedObject.FindProperty(PROP_OPEN);
            SerializedProperty start = this.serializedObject.FindProperty(PROP_START);
            SerializedProperty finish = this.serializedObject.FindProperty(PROP_FINISH);
            SerializedProperty log = this.serializedObject.FindProperty(PROP_LOG);

            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("Animations"));
            root.Add(new PropertyField(when));
            root.Add(new PropertyField(idle));
            root.Add(new PropertyField(open));
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("Sound Effects"));
            root.Add(new PropertyField(start));
            root.Add(new PropertyField(finish));
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("Override"));
            root.Add(new PropertyField(log));

            return root;
        }
        
        // CREATE ASSET METHODS: ------------------------------------------------------------------
        
        [MenuItem("Assets/Create/Game Creator/Dialogue/Speech Skin", false, 0)]
        internal static void CreateFromMenuItem()
        {
            SpeechSkin skin = CreateInstance<SpeechSkin>();

            string selection = Selection.activeObject != null
                ? AssetDatabase.GetAssetPath(Selection.activeObject)
                : ASSETS;

            string directory = File.Exists(PathUtils.PathForOS(selection)) 
                ? PathUtils.PathToUnix(Path.GetDirectoryName(selection)) 
                : selection;

            string path = AssetDatabase.GenerateUniqueAssetPath(
                PathUtils.Combine(directory ?? ASSETS, "Speech.asset")
            );
            
            AssetDatabase.CreateAsset(skin, path);
            AssetDatabase.SaveAssets();
            
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = skin;

            AnimatorOverrideController controller = Instantiate(
                AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(CONTROLLER_PATH)
            );

            controller.name = Path.GetFileNameWithoutExtension(CONTROLLER_PATH); 
            controller.hideFlags = HideFlags.HideInHierarchy;
            
            AssetDatabase.AddObjectToAsset(controller, skin);
            typeof(SpeechSkin)
                .GetField(PROP_CONTROLLER, MEMBER_FLAGS)?
                .SetValue(skin, controller);

            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(skin));
            
            skin.name = "Speech";
        }
    }
}