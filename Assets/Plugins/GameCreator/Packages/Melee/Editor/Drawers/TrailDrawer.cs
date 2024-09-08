using GameCreator.Editor.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(Trail))]
    public class TrailDrawer : TBoxDrawer
    {
        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty pointA = property.FindPropertyRelative("m_PointA");
            SerializedProperty pointB = property.FindPropertyRelative("m_PointB");
            
            SerializedProperty quads = property.FindPropertyRelative("m_Quads");
            SerializedProperty length = property.FindPropertyRelative("m_Length");
            SerializedProperty material = property.FindPropertyRelative("m_Material");
            
            container.Add(new PropertyField(pointA));
            container.Add(new PropertyField(pointB));
            
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(quads));
            container.Add(new PropertyField(length));
            container.Add(new PropertyField(material));
        }
    }
}