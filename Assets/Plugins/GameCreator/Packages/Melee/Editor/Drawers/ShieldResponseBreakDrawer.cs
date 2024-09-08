using GameCreator.Runtime.Melee;
using UnityEditor;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(ShieldResponseBreak))]
    public class ShieldResponseBreakDrawer : TShieldResponseDrawer
    {
        protected override string Type => "Break";
    }
}