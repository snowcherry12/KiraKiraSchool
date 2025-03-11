using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(ReloadList))]
    public class ReloadListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty state = property.FindPropertyRelative("m_State");
            SerializedProperty layer = property.FindPropertyRelative("m_Layer");

            PropertyField fieldState = new PropertyField(state);
            PropertyField fieldLayer = new PropertyField(layer);
            
            root.Add(fieldState);
            root.Add(new SpaceSmaller());
            root.Add(fieldLayer);
            root.Add(new SpaceSmaller());
            
            ReloadListTool reloadListTool = new ReloadListTool(property);
            root.Add(reloadListTool);
            
            return root;
        }
    }
}
