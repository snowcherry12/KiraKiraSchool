using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEditor;

namespace GameCreator.Editor.Inventory.UnityUI
{
    [CustomPropertyDrawer(typeof(TinkerDismantlingUI))]
    public class TinkerDismantlingUIDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Dismantle Items";
    }
}