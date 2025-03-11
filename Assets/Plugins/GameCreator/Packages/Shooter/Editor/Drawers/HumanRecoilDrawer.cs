using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(HumanRecoil))]
    public class HumanRecoilDrawer : TSectionDrawer
    {
        protected override string Name(SerializedProperty property) => property.displayName;
    }
}