using System;
using UnityEngine;
using UnityEngine.AI;

namespace GameCreator.Runtime.Characters
{
    [Serializable]
    public class DriverNavmeshArea
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private int m_Area;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public int Area => this.m_Area;
    }
}