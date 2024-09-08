using GameCreator.Editor.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(SkillCharge))]
    public class SkillChargeDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Charge";
    }
}