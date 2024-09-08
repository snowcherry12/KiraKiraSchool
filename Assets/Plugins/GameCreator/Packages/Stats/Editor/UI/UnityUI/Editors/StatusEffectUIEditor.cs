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
    [CustomEditor(typeof(StatusEffectUI))]
    public class StatusEffectUIEditor : UnityEditor.Editor
    {
        private const string MSG = "This component is called from a Status Effect List UI";
        
        // MEMBERS: -------------------------------------------------------------------------------

        private VisualElement m_Root;
        
        // PAINT METHOD: --------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();

            SerializedProperty common = this.serializedObject.FindProperty("m_Common");
            SerializedProperty count = this.serializedObject.FindProperty("m_Count");
            SerializedProperty remainingTime = this.serializedObject.FindProperty("m_RemainingTime");
            
            SerializedProperty imageFill = this.serializedObject.FindProperty("m_ImageFill");
            SerializedProperty scaleX = this.serializedObject.FindProperty("m_ScaleX");
            SerializedProperty scaleY = this.serializedObject.FindProperty("m_ScaleY");

            this.m_Root.Add(new InfoMessage(MSG));
            this.m_Root.Add(new PropertyField(common));
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new LabelTitle("Values:"));
            this.m_Root.Add(new PropertyField(count));
            this.m_Root.Add(new PropertyField(remainingTime));
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new LabelTitle("Duration:"));
            this.m_Root.Add(new PropertyField(imageFill));
            this.m_Root.Add(new PropertyField(scaleX));
            this.m_Root.Add(new PropertyField(scaleY));

            return this.m_Root;
        }
        
        // CREATION MENU: -------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/UI/Stats/Status Effect UI", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject canvas = UnityUIUtilities.GetCanvas();
            
            DefaultControls.Resources resources = UnityUIUtilities.GetStandardResources();
            GameObject gameObject = DefaultControls.CreateImage(resources);
            gameObject.transform.SetParent(canvas.transform, false);
            gameObject.name = "Status Effect UI";

            UnityEngine.UI.Image image = gameObject.GetComponent<UnityEngine.UI.Image>();
            image.fillMethod = UnityEngine.UI.Image.FillMethod.Horizontal;
            image.fillAmount = 0.75f;
            
            StatusEffectUI.CreateFrom(image);

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create {gameObject.name}");
            Selection.activeGameObject = gameObject;
        }
    }
}