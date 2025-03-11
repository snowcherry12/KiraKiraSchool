using GameCreator.Editor.Common;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    public class CrosshairRangeField : Vector2Field
    {
        private const string SPACER = "unity-composite-field__field-spacer";
        private static readonly Length GAP = new Length(2, LengthUnit.Pixel);
        
        public CrosshairRangeField(SerializedProperty property) : base(property.displayName)
        {
            FloatField fieldMin = this.Q<FloatField>("unity-x-input");
            FloatField fieldMax = this.Q<FloatField>("unity-y-input");

            fieldMin.label = string.Empty;
            fieldMax.label = string.Empty;

            fieldMin.style.marginRight = GAP;
            
            VisualElement spacer = this.Q<VisualElement>(className: SPACER);
            spacer?.parent.Remove(spacer);

            this.AddToClassList(AlignLabel.CLASS_UNITY_ALIGN_LABEL);
            
            this.bindingPath = property.propertyPath;
        }
    }
}