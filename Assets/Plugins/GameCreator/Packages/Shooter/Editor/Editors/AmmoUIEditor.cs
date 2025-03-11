using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomEditor(typeof(AmmoUI))]
    public class AmmoUIEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            root.Add(new PropertyField(this.serializedObject.FindProperty("m_Character")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_Weapon")));
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_InMagazine")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_InMunition")));
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_MagazineFill")));
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_PrefabInMagazine")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_ContentInMagazine")));
            
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_ActiveIfEmpty")));
            root.Add(new PropertyField(this.serializedObject.FindProperty("m_ActiveIfFull")));
            
            return root;
        }
    }
}