using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(Magazine))]
    public class MagazineDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Magazine";
    }
}