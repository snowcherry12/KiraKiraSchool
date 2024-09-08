using GameCreator.Editor.Common;
using GameCreator.Editor.Common.UnityUI;
using GameCreator.Runtime.Stats.UnityUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats.UnityUI
{
    [CustomEditor(typeof(StatusEffectListUI))]
    public class StatusEffectListUIEditor : UnityEditor.Editor
    {
        // MEMBERS: -------------------------------------------------------------------------------

        private VisualElement m_Root;
        
        // PAINT METHOD: --------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            SerializedProperty statusEffectTarget = this.serializedObject.FindProperty("m_Target");
            SerializedProperty types = this.serializedObject.FindProperty("m_Types");
            SerializedProperty showHidden = this.serializedObject.FindProperty("m_ShowHidden");
            
            SerializedProperty container = this.serializedObject.FindProperty("m_Container");
            SerializedProperty prefab = this.serializedObject.FindProperty("m_PrefabStatusEffect");

            this.m_Root.Add(new PropertyField(statusEffectTarget));
            this.m_Root.Add(new PropertyField(types));
            this.m_Root.Add(new PropertyField(showHidden));
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(container));
            this.m_Root.Add(new PropertyField(prefab));

            return this.m_Root;
        }
        
        // CREATION MENU: -------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/UI/Stats/Status Effect List UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateButton(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "Status Effect List UI";

            DestroyImmediate(gameObject.GetComponent<Text>());
            gameObject.AddComponent<StatusEffectListUI>();

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}