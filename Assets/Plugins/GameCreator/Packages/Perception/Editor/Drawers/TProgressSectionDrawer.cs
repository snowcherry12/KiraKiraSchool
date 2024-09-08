using GameCreator.Editor.Common;
using GameCreator.Runtime.Perception.UnityUI;
using UnityEditor;

namespace GameCreator.Editor.Perception
{
    [CustomPropertyDrawer(typeof(TProgressSection), true)]
    public class TProgressSectionDrawer : TBoxDrawer
    { }
}