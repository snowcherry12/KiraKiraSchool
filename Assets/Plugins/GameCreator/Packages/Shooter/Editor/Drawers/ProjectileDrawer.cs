using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(Projectile))]
    public class ProjectileDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Projectile";
    }
}