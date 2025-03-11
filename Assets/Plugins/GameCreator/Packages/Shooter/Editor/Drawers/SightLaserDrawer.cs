using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(SightLaser))]
    public class SightLaserDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Laser";
    }
}