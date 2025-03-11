using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomEditor(typeof(ReloadUI))]
    public class ReloadUIEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            root.Add(new PropertyField(this.serializedObject.FindProperty("m_Character")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_Weapon")));
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_ActiveIfReloading")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_ActiveInQuickReloadRange")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_ActiveFailQuickReload")));
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_ReloadProgressFill")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_ReloadProgressScaleX")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_ReloadProgressScaleY")));
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_QuickReloadRange")));
            
            return root;
        }
    }
}