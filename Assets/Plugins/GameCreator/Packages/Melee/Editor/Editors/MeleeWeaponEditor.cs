using GameCreator.Editor.Characters;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomEditor(typeof(MeleeWeapon))]
    public class MeleeWeaponEditor : TWeaponEditor
    {
        protected override bool HasShieldMember => true;
        
        protected override bool HasParriedReaction => true;
        protected override bool HasDodge => true;
        
        protected override void CreateGUI(VisualElement root)
        {
            VisualElement content = new VisualElement();
            content.AddToClassList(CLASS_MARGIN_X);
            content.AddToClassList(CLASS_MARGIN_Y);

            root.Add(content);
            
            SerializedProperty state = this.serializedObject.FindProperty("m_State");
            SerializedProperty layer = this.serializedObject.FindProperty("m_Layer");
            
            content.Add(new SpaceSmall());
            content.Add(new PropertyField(state));
            content.Add(new PropertyField(layer));
            
            root.Add(new SpaceSmall());
            
            SerializedProperty combos = this.serializedObject.FindProperty("m_Combos");
            root.Add(new PropertyField(combos));
        }
    }
}