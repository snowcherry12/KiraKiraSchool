using GameCreator.Runtime.Melee;
using UnityEditor;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(ShieldResponseBlock))]
    public class ShieldResponseBlockDrawer : TShieldResponseDrawer
    {
        protected override string Type => "Block";
    }
}