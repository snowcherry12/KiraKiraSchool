using GameCreator.Editor.Common;
using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(Score))]
    public class ScoreDrawer : PropertyDrawer
    {
        public const string PROP_VALUE = "m_Value";
        public const string PROP_CURVE = "m_Curve";
        
        public static readonly Length HEIGHT = new Length(60f, LengthUnit.Pixel);
        
        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            ContentBox root = new ContentBox("Score", true);

            SerializedProperty value = property.FindPropertyRelative(PROP_VALUE);

            CurveField fieldCurve = CreateCurveField();
            fieldCurve.bindingPath = property.FindPropertyRelative(PROP_CURVE).propertyPath;

            PropertyField fieldValue = new PropertyField(value, "Score");

            root.Content.Add(fieldCurve);
            root.Content.Add(new SpaceSmall());
            root.Content.Add(fieldValue);
            
            return root;
        }

        public static CurveField CreateCurveField()
        {
            return new CurveField
            {
                label = string.Empty,
                renderMode = CurveField.RenderMode.Mesh,
                style =
                {
                    marginBottom = 0f,
                    marginTop = 0f,
                    marginLeft = 0f,
                    marginRight = 0f,
                    height = HEIGHT
                }
            };
        }
    }
}