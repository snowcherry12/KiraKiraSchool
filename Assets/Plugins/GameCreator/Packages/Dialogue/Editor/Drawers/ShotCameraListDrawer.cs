using GameCreator.Editor.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;

namespace GameCreator.Editor.Dialogue
{
    [CustomPropertyDrawer(typeof(ShotCameraList))]
    public class ShotCameraListDrawer : TArrayDrawer
    {
        protected override string PropertyArrayName => "m_Shots";
        protected override float ItemHeight => 22;
    }
}