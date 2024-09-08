using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(ComboSelector))]
    public class ComboSelectorDrawer : PropertyDrawer
    {
        private const string EMPTY = " ";
        private const string CLASS_MARGIN_X = "gc-inspector-margins-x";
        private const string CLASS_MARGIN_Y = "gc-margin-top-default";

        private const string NAME_COMBO = "Combos";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty combosFrom = property.FindPropertyRelative("m_CombosFrom");
            SerializedProperty combosAsset = property.FindPropertyRelative("m_CombosAsset");
            SerializedProperty combosEmbed = property.FindPropertyRelative("m_CombosEmbed");
            
            PropertyField fieldCombosFrom = new PropertyField(combosFrom, NAME_COMBO);
            PropertyField fieldCombosAsset = new PropertyField(combosAsset, EMPTY);
            PropertyField fieldCombosEmbed = new PropertyField(combosEmbed, EMPTY);
            
            fieldCombosFrom.AddToClassList(CLASS_MARGIN_X);
            fieldCombosAsset.AddToClassList(CLASS_MARGIN_X);
            fieldCombosEmbed.AddToClassList(CLASS_MARGIN_Y);
            
            root.Add(fieldCombosFrom);
            root.Add(fieldCombosAsset);
            root.Add(fieldCombosEmbed);

            fieldCombosAsset.style.display = combosFrom.enumValueIndex switch
            {
                0 => DisplayStyle.Flex,
                1 => DisplayStyle.None,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            fieldCombosEmbed.style.display = combosFrom.enumValueIndex switch
            {
                0 => DisplayStyle.None,
                1 => DisplayStyle.Flex,
                _ => throw new ArgumentOutOfRangeException()
            };

            fieldCombosFrom.RegisterValueChangeCallback(changeEvent =>
            {
                fieldCombosAsset.style.display = changeEvent.changedProperty.enumValueIndex switch
                {
                    0 => DisplayStyle.Flex,
                    1 => DisplayStyle.None,
                    _ => throw new ArgumentOutOfRangeException()
                };

                fieldCombosEmbed.style.display = changeEvent.changedProperty.enumValueIndex switch
                {
                    0 => DisplayStyle.None,
                    1 => DisplayStyle.Flex,
                    _ => throw new ArgumentOutOfRangeException()
                };
            });
            
            return root;
        }
    }
}