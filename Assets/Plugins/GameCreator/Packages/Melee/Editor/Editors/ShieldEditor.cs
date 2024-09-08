using GameCreator.Editor.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomEditor(typeof(Shield))]
    public class ShieldEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement content = new VisualElement();

            SerializedProperty angle = this.serializedObject.FindProperty("m_Angle");
            SerializedProperty parry = this.serializedObject.FindProperty("m_ParryTime");
            SerializedProperty defense = this.serializedObject.FindProperty("m_Defense");
            SerializedProperty cooldown = this.serializedObject.FindProperty("m_Cooldown");
            SerializedProperty recovery = this.serializedObject.FindProperty("m_Recovery");

            content.Add(new SpaceSmall());
            content.Add(new PropertyField(angle));
            content.Add(new SpaceSmallest());
            content.Add(new PropertyField(parry));
            content.Add(new SpaceSmall());
            content.Add(new PropertyField(defense));
            content.Add(new SpaceSmall());
            content.Add(new PropertyField(cooldown));
            content.Add(new SpaceSmallest());
            content.Add(new PropertyField(recovery));

            SerializedProperty state = this.serializedObject.FindProperty("m_State");
            SerializedProperty layer = this.serializedObject.FindProperty("m_Layer");

            PadBox animationBox = new PadBox();
            
            content.Add(new SpaceSmall());
            content.Add(animationBox);
            
            animationBox.Add(new PropertyField(state));
            animationBox.Add(new PropertyField(layer));
            
            SerializedProperty speed = this.serializedObject.FindProperty("m_Speed");

            animationBox.Add(new SpaceSmall());
            animationBox.Add(new PropertyField(speed));

            SerializedProperty transIn = this.serializedObject.FindProperty("m_TransitionIn");
            SerializedProperty transOut = this.serializedObject.FindProperty("m_TransitionOut");
            
            animationBox.Add(new SpaceSmall());
            animationBox.Add(new PropertyField(transIn));
            animationBox.Add(new PropertyField(transOut));
            
            SerializedProperty blockResponse = this.serializedObject.FindProperty("m_Block");
            SerializedProperty parryResponse = this.serializedObject.FindProperty("m_Parry");
            SerializedProperty breakResponse = this.serializedObject.FindProperty("m_Break");
            
            content.Add(new SpaceSmall());
            content.Add(new PropertyField(blockResponse));
            content.Add(new SpaceSmallest());
            content.Add(new PropertyField(parryResponse));
            content.Add(new SpaceSmallest());
            content.Add(new PropertyField(breakResponse));

            return content;
        }
    }
}