using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [CreateAssetMenu(
        fileName = "Reaction (Melee)", 
        menuName = "Game Creator/Melee/Reaction",
        order    = 50
    )]
    
    [Icon(EditorPaths.PACKAGES + "Melee/Editor/Gizmos/GizmoMeleeReaction.png")]
    
    [Serializable]
    public class MeleeReaction : Reaction
    { }
}