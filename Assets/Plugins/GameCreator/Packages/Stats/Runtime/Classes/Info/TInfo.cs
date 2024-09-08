using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.Stats
{
    [Serializable]
    public abstract class TInfo
    {
        [SerializeField] [FormerlySerializedAs("name")] public PropertyGetString m_Name;
        [SerializeField] [FormerlySerializedAs("acronym")] public PropertyGetString m_Acronym;
        [SerializeField] [FormerlySerializedAs("description")] public PropertyGetString m_Description;
        
        [SerializeField] [FormerlySerializedAs("icon")] public PropertyGetSprite m_Icon;
        [SerializeField] [FormerlySerializedAs("color")] public Color m_Color;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TInfo()
        {
            this.m_Icon = GetSpriteNone.Create;
            this.m_Color = Color.white;
        }
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Color Color => this.m_Color;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public string GetName(Args args) => this.m_Name.Get(args);
        public string GetAcronym(Args args) => this.m_Acronym.Get(args);
        public string GetDescription(Args args) => this.m_Description.Get(args);
        public Sprite GetIcon(Args args) => this.m_Icon.Get(args);
    }
}