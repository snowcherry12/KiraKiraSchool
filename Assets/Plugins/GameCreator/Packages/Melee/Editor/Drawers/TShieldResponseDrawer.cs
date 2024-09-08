using GameCreator.Editor.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    public abstract class TShieldResponseDrawer : TBoxDrawer
    {
        protected abstract string Type { get; }
        
        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty reaction = property.FindPropertyRelative("m_Reaction");
            SerializedProperty effect = property.FindPropertyRelative("m_Effect");
            SerializedProperty instructionsList = property.FindPropertyRelative("m_InstructionsList");
            
            container.Add(new PropertyField(reaction, $"{this.Type} Reaction"));
            container.Add(new PropertyField(effect, $"{this.Type} Effect"));
            
            container.Add(new SpaceSmall());
            container.Add(new LabelTitle($"On {this.Type}:"));
            container.Add(new SpaceSmaller());
            container.Add(new PropertyField(instructionsList));
        }
    }
}