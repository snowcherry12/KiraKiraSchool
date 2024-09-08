using GameCreator.Runtime.Perception;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    [CustomPropertyDrawer(typeof(SensorList))]
    public class SensorListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SensorListTool sensorListTool = new SensorListTool(property);
            return sensorListTool;
        }
    }
}