using GameCreator.Editor.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomEditor(typeof(CanHit))]
    public class CanHitEditor : UnityEditor.Editor
    {
        private const string TXT = "Skills can hit this game object";
        private const string ERR_NO_COLLIDER = "This component requires a Collider";
        
        public override VisualElement CreateInspectorGUI()
        {
            return (this.target as Component)?.GetComponent<Collider>() != null
                ? new InfoMessage(TXT)
                : new ErrorMessage(ERR_NO_COLLIDER);
        }
    }
}