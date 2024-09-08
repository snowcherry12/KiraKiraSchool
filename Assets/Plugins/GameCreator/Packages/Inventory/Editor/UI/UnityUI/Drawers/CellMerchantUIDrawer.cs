using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEditor;

namespace GameCreator.Editor.Inventory.UnityUI
{
    [CustomPropertyDrawer(typeof(CellMerchantUI))]
    public class CellMerchantUIDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Merchant Info";
    }
}
