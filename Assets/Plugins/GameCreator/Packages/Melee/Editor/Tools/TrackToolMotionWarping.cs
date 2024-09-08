using GameCreator.Editor.Common;
using GameCreator.Editor.VisualScripting;
using GameCreator.Runtime.Melee;
using UnityEditor;

namespace GameCreator.Editor.Melee
{
    public class TrackToolMotionWarping : TrackTool
    {
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public TrackToolMotionWarping(SequenceTool sequenceTool, int trackIndex) 
            : base(sequenceTool, trackIndex)
        { }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------
        
        protected override void OnCreateClip(SerializedProperty clip, float time, float duration)
        {
            clip.SetValue(new ClipMeleeMotionWarping());
            clip.FindPropertyRelative("m_Time").floatValue = time;
            clip.FindPropertyRelative("m_Duration").floatValue = duration;
        }
    }
}