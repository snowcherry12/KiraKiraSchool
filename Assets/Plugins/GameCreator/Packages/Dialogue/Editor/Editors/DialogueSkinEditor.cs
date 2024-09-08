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
    [CustomEditor(typeof(DialogueSkin))]
    public class DialogueSkinEditor : SkinEditor
    {
        private const BindingFlags MEMBER_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;
        
        private const string ASSETS = "Assets/";
        
        private const string CONTROLLER_PATH = 
            RuntimePaths.PACKAGES + 
            "Dialogue/Runtime/Assets/Overrides/Dialogue.overrideController";

        private const string PROP_CONTROLLER = "m_Controller";
        private const string PROP_IDLE = "m_Idle";
        private const string PROP_OPEN = "m_Open";
        private const string PROP_CLOSE = "m_Close";
        
        private const string PROP_START = "m_Start";
        private const string PROP_FINISH = "m_Finish";
        private const string PROP_SELECT = "m_Select";
        private const string PROP_SUBMIT = "m_Submit";
        
        private const string PROP_VALUES_CHOICES = "m_ValuesChoices";
        private const string PROP_VALUES_RANDOM = "m_ValuesRandom";
        
        // PAINT METHOD: --------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = base.CreateInspectorGUI();
            
            SerializedProperty idle = this.serializedObject.FindProperty(PROP_IDLE);
            SerializedProperty open = this.serializedObject.FindProperty(PROP_OPEN);
            SerializedProperty close = this.serializedObject.FindProperty(PROP_CLOSE);
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("Animations"));
            root.Add(new PropertyField(idle));
            root.Add(new PropertyField(open));
            root.Add(new PropertyField(close));
            
            SerializedProperty start = this.serializedObject.FindProperty(PROP_START);
            SerializedProperty finish = this.serializedObject.FindProperty(PROP_FINISH);
            SerializedProperty select = this.serializedObject.FindProperty(PROP_SELECT);
            SerializedProperty submit = this.serializedObject.FindProperty(PROP_SUBMIT);
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("Sound Effects"));
            root.Add(new PropertyField(start));
            root.Add(new PropertyField(finish));
            root.Add(new PropertyField(select));
            root.Add(new PropertyField(submit));
            
            SerializedProperty valuesChoices = this.serializedObject.FindProperty(PROP_VALUES_CHOICES);
            SerializedProperty valuesRandom = this.serializedObject.FindProperty(PROP_VALUES_RANDOM);
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(valuesChoices));
            root.Add(new PropertyField(valuesRandom));

            return root;
        }
        
        // CREATE ASSET METHODS: ------------------------------------------------------------------
        
        [MenuItem("Assets/Create/Game Creator/Dialogue/Dialogue Skin", false, 0)]
        internal static void CreateFromMenuItem()
        {
            DialogueSkin skin = CreateInstance<DialogueSkin>();

            string selection = Selection.activeObject != null
                ? AssetDatabase.GetAssetPath(Selection.activeObject)
                : ASSETS;

            string directory = File.Exists(PathUtils.PathForOS(selection)) 
                ? PathUtils.PathToUnix(Path.GetDirectoryName(selection)) 
                : selection;

            string path = AssetDatabase.GenerateUniqueAssetPath(
                PathUtils.Combine(directory ?? ASSETS, "Dialogue.asset")
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
            typeof(DialogueSkin)
                .GetField(PROP_CONTROLLER, MEMBER_FLAGS)?
                .SetValue(skin, controller);

            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(skin));
            
            skin.name = "Dialogue";
        }
    }
}