using GameCreator.Editor.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory.UnityUI
{
    public abstract class TTinkerItemUIEditor : UnityEditor.Editor
    {
        protected abstract string LabelButtonTinker { get; }
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty buttonSelect = this.serializedObject.FindProperty("m_ButtonSelect");
            SerializedProperty activeIfSelected = this.serializedObject.FindProperty("m_ActiveIfSelected");
            SerializedProperty duration = this.serializedObject.FindProperty("m_Duration");
            SerializedProperty itemUI = this.serializedObject.FindProperty("m_ItemUI");
            SerializedProperty canTinker = this.serializedObject.FindProperty("m_ActiveIfCanTinker");
            SerializedProperty buttonTinker = this.serializedObject.FindProperty("m_ButtonTinker");
            SerializedProperty amountInInputBag = this.serializedObject.FindProperty("m_AmountInInputBag");
            SerializedProperty amountInOutputBag = this.serializedObject.FindProperty("m_AmountInOutputBag");
            SerializedProperty canProgress = this.serializedObject.FindProperty("m_ActiveIfInProgress");
            SerializedProperty progress = this.serializedObject.FindProperty("m_Progress");
            SerializedProperty prefab = this.serializedObject.FindProperty("m_PrefabIngredientUI");
            SerializedProperty content = this.serializedObject.FindProperty("m_IngredientsContent");

            SerializedProperty onStart = this.serializedObject.FindProperty("m_OnStart");
            SerializedProperty onComplete = this.serializedObject.FindProperty("m_OnComplete");
            
            root.Add(new PropertyField(buttonSelect));
            root.Add(new PropertyField(activeIfSelected));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(duration));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(itemUI));
            root.Add(new SpaceSmaller());
            root.Add(new PropertyField(canTinker));
            root.Add(new PropertyField(buttonTinker, this.LabelButtonTinker));
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(amountInInputBag));
            root.Add(new PropertyField(amountInOutputBag));
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(canProgress));
            root.Add(new PropertyField(progress));
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(prefab));
            root.Add(new PropertyField(content));
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Start"));
            root.Add(new SpaceSmallest());
            root.Add(new PropertyField(onStart));
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("On Complete"));
            root.Add(new SpaceSmallest());
            root.Add(new PropertyField(onComplete));
            
            return root;
        }
    }
}