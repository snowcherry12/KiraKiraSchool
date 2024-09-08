using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [AddComponentMenu("Game Creator/Melee/Can Hit")]
    [Icon(RuntimePaths.PACKAGES + "Melee/Editor/Gizmos/GizmoCanHit.png")]
    
    [Serializable]
    public class CanHit : MonoBehaviour
    {
        public virtual bool AllowHits(Character attacker)
        {
            return this.isActiveAndEnabled;
        }
    }
}
