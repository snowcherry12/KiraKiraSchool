using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEditor;

namespace GameCreator.Editor.Inventory.UnityUI
{
    [CustomPropertyDrawer(typeof(TinkerCraftingUI))]
    public class TinkerCraftingUIDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Craft Items";
    }
}