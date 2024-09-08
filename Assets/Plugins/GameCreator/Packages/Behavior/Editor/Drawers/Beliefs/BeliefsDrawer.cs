using GameCreator.Runtime.Behavior;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Behavior
{
    [CustomPropertyDrawer(typeof(Beliefs))]
    public class BeliefsDrawer : PropertyDrawer
    {
        public const string PROP_LIST = "m_List";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            BeliefsTool beliefsTool = new BeliefsTool(property);
            return beliefsTool;
        }
    }
}