using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [CreateAssetMenu(
        fileName = "Combos", 
        menuName = "Game Creator/Melee/Combos",
        order    = 50
    )]
    
    [Icon(EditorPaths.PACKAGES + "Melee/Editor/Gizmos/GizmoCombos.png")]
    
    public class Combos : ScriptableObject
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeReference] private ComboTree m_Combos = new ComboTree();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public ComboTree Get => this.m_Combos;
    }
}