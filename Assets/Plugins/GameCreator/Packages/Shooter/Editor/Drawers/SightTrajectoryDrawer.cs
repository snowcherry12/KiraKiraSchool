using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(SightTrajectory))]
    public class SightTrajectoryDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Trajectory";
    }
}