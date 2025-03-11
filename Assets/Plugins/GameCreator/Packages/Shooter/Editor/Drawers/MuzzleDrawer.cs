using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(Muzzle))]
    public class MuzzleDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Muzzle";
    }
}