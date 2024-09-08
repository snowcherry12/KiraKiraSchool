using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    public class TypeSelectorSensor : TypeSelectorListFancy
    {
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TypeSelectorSensor(SerializedProperty propertyList, Button element)
            : base(propertyList, typeof(TSensor), element)
        { }
    }
}