using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Perception;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Perception
{
    public class TypeSelectorElementSensor : Button
    {
        private static readonly IIcon ICON_ADD = new IconSensor(ColorTheme.Type.TextLight);

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public TypeSelectorElementSensor(SerializedProperty propertyList, SensorListTool tool)
        {
            this.Add(new Image { image = ICON_ADD.Texture });
            this.Add(new Label { text = "Add Sensor..." });
            
            TypeSelectorSensor typeSelector = new TypeSelectorSensor(propertyList, this);
            typeSelector.EventChange += (prevType, newType) =>
            {
                object instance = Activator.CreateInstance(newType);
                tool.InsertItem(propertyList.arraySize, instance);
            };
        }
    }
}