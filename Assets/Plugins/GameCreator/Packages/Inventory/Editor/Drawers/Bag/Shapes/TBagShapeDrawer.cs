using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    public abstract class TBagShapeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Bag bag = property.serializedObject.targetObject as Bag;
                if (bag != null) this.RuntimeContent(bag.Shape, root);
            }
            else
            {
                this.EditorContent(property, root);   
            }

            return root;
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------
        
        protected abstract void EditorContent(SerializedProperty property, VisualElement root);
        protected abstract void RuntimeContent(IBagShape shape, VisualElement root);
    }
}