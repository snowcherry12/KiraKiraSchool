using GameCreator.Runtime.Melee;
using UnityEditor;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(ShieldResponseParry))]
    public class ShieldResponseParryDrawer : TShieldResponseDrawer
    {
        protected override string Type => "Parry";
    }
}