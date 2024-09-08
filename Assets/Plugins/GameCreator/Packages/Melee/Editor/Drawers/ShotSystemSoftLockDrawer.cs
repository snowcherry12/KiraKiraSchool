using GameCreator.Editor.Cameras;
using GameCreator.Runtime.Melee;
using UnityEditor;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(ShotSystemSoftLock))]
    public class ShotSystemSoftLockDrawer : TShotSystemDrawer
    {
        protected override string Name(SerializedProperty property) => "Soft Lock";
    }
}