using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(ComboTree))]
    public class ComboTreeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new CombosTool(property);
        }
    }
}