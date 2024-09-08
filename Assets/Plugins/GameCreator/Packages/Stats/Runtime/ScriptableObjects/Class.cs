using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [CreateAssetMenu(
        fileName = "Class", 
        menuName = "Game Creator/Stats/Class",
        order    = 50
    )]
    
    [Icon(EditorPaths.PACKAGES + "Stats/Editor/Gizmos/GizmoClass.png")]
    
    public class Class : ScriptableObject
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetString m_Class = GetStringSelfName.Create;
        [SerializeField] private PropertyGetString m_Description = GetStringTextArea.Create();

        [SerializeField] private PropertyGetSprite m_Sprite = GetSpriteNone.Create;
        [SerializeField] private PropertyGetColor m_Color = GetColorColorsWhite.Create;
        
        [SerializeField] private AttributeList m_Attributes = new AttributeList();
        [SerializeField] private StatList m_Stats = new StatList();

        // PROPERTIES: ----------------------------------------------------------------------------

        public int AttributesLength => this.m_Attributes.Length;
        public int StatsLength => this.m_Stats.Length;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public string GetName(Args args) => this.m_Class.Get(args);
        public string GetDescription(Args args) => this.m_Description.Get(args);
        
        public Sprite GetSprite(Args args) => this.m_Sprite.Get(args);
        public Color GetColor(Args args) => this.m_Color.Get(args);
        
        public AttributeItem GetAttribute(int index) => this.m_Attributes.Get(index);
        public StatItem GetStat(int index) => this.m_Stats.Get(index);
    }
}