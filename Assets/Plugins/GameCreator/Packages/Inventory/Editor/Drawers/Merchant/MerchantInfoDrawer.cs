using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(MerchantInfo))]
    public class MerchantInfoDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Merchant Info";
    }
}