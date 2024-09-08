using System;
using UnityEngine;

namespace GameCreator.Runtime.Melee
{
    [Serializable]
    public class ComboSelector
    {
        private enum CombosFrom
        {
            Asset,
            Embedded
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] 
        private CombosFrom m_CombosFrom = CombosFrom.Asset;
        
        [SerializeField]
        private Combos m_CombosAsset;
        
        [SerializeReference]
        private ComboTree m_CombosEmbed = new ComboTree();

        // PROPERTIES: ----------------------------------------------------------------------------

        public ComboTree Get => this.m_CombosFrom switch
        {
            CombosFrom.Asset => this.m_CombosAsset != null ? this.m_CombosAsset.Get : null,
            CombosFrom.Embedded => this.m_CombosEmbed,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}