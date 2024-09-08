using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    public class ExpressingTool : VisualElement
    {
        private const string SPACE = " ";
        
        // MEMBERS: -------------------------------------------------------------------------------

        private readonly SerializedProperty m_Actor;
        private readonly SerializedProperty m_Expression;
        
        private readonly PropertyField m_FieldActor;
        private readonly PopupField<int> m_FieldExpression;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public ExpressingTool(SerializedProperty property)
        {
            this.m_Actor = property.FindPropertyRelative(ExpressingDrawer.PROPERTY_ACTOR);
            this.m_Expression = property.FindPropertyRelative(ExpressingDrawer.PROPERTY_EXPRESSION);

            this.m_FieldActor = new PropertyField(this.m_Actor);
            this.m_FieldExpression = new PopupField<int>(
                SPACE,
                GetChoices(this.m_Actor.objectReferenceValue as Actor),
                this.m_Expression.intValue,
                this.PrintSelection,
                this.PrintListItem
            );

            AlignLabel.On(this.m_FieldExpression);
            
            this.m_FieldActor.RegisterValueChangeCallback(changeEvent =>
            {
                Actor actorAsset = changeEvent.changedProperty.objectReferenceValue as Actor;
                
                this.m_FieldExpression.choices = GetChoices(actorAsset);
                this.m_FieldExpression.style.display = actorAsset != null 
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            }); 

            this.m_FieldExpression.RegisterValueChangedCallback(changeEvent =>
            {
                this.m_Expression.intValue = changeEvent.newValue;
                
                this.m_Expression.serializedObject.ApplyModifiedProperties();
                this.m_Expression.serializedObject.Update();
            });

            this.Add(this.m_FieldActor);
            this.Add(this.m_FieldExpression);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private string PrintSelection(int index)
        {
            Actor actor = this.m_Actor.objectReferenceValue as Actor;
            string text = actor != null
                ? actor.GetExpressionFromIndex(index)?.ToString()
                : string.Empty; 
            
            return TextUtils.Humanize(text);
        }
        
        private string PrintListItem(int index)
        {
            Actor actor = this.m_Actor.objectReferenceValue as Actor;
            return actor != null
                ? $"{index}: {TextUtils.Humanize(actor.GetExpressionFromIndex(index))}" 
                : string.Empty;
        }
        
        // PRIVATE STATIC METHODS: ----------------------------------------------------------------
        
        private static List<int> GetChoices(Actor actor)
        {
            if (actor == null) return new List<int>();

            List<int> expressions = new List<int>();
            int length = actor.ExpressionsLength;
            
            for (int i = 0; i < length; ++i) expressions.Add(i);
            return expressions;
        }
    }
}