using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(SightCrosshair))]
    public class SightCrosshairDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Crosshair";
    }
}