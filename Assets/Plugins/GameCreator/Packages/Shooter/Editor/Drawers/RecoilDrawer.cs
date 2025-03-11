using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(Recoil))]
    public class RecoilDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Recoil";
    }
}