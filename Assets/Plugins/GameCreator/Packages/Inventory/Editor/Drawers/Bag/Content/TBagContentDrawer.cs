using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(TBagContent), true)]
    public class TBagContentDrawer : PropertyDrawer
    {
        private const string NAME_ITEM_ROOT = "GC-Inventory-Bag-Content-Item-Root";
        private const string NAME_ITEM_HEAD = "GC-Inventory-Bag-Content-Item-Head";
        private const string NAME_ITEM_BODY = "GC-Inventory-Bag-Content-Item-Body";
        
        // PAINT METHOD: --------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Bag bag = property.serializedObject.targetObject as Bag;
                if (bag != null) this.RuntimeContent(bag.Content, root);
            }
            else
            {
                this.EditorContent(property, root);   
            }

            return root;
        }
        
        // EDITOR: --------------------------------------------------------------------------------

        protected virtual void EditorContent(SerializedProperty property, VisualElement root)
        { }

        // RUNTIME: -------------------------------------------------------------------------------
        
        protected virtual void RuntimeContent(IBagContent content, VisualElement root)
        {
            List<Cell> itemList = content.CellList;
            if (itemList.Count == 0) return;
            
            root.Add(new SpaceSmall());
            root.Add(new LabelTitle("Content"));
            root.Add(new SpaceSmaller());

            foreach (Cell cell in itemList)
            {
                if (cell == null || cell.Available) continue;
                VisualElement element = new VisualElement { name = NAME_ITEM_ROOT };

                Button head = new Button
                {
                    name = NAME_ITEM_HEAD,
                    text = $"{cell.Item.ID.String} ({cell.Count})"
                };

                VisualElement body = new VisualElement
                {
                    name = NAME_ITEM_BODY,
                    style = { display = DisplayStyle.None }
                };

                head.clicked += () => body.style.display = body.style.display == DisplayStyle.Flex
                    ? DisplayStyle.None
                    : DisplayStyle.Flex;
                
                List<IdString> stack = cell.List;
                foreach (IdString runtimeItemID in stack)
                {
                    body.Add(new Label(runtimeItemID.String));
                }
                
                element.Add(head);
                element.Add(body);
                
                root.Add(element);
            }
        }
    }
}