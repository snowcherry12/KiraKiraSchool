using GameCreator.Runtime.Common;
using GameCreator.Runtime.Dialogue.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [CreateAssetMenu(
        fileName = "Actor", 
        menuName = "Game Creator/Dialogue/Actor"
    )]
    
    [Icon(EditorPaths.PACKAGES + "Dialogue/Editor/Gizmos/GizmoActor.png")]
    
    public class Actor : ScriptableObject
    {
        [SerializeField] private Actant m_Actant = new Actant();
        [SerializeField] private Expressions m_Expressions = new Expressions();
        [SerializeField] private Typewriter m_Typewriter = new Typewriter();
        [SerializeField] private SpeechSkin m_OverrideSpeechSkin;
        [SerializeField] private Portrait m_Portrait = Portrait.Primary;

        // PROPERTIES: ----------------------------------------------------------------------------

        public int ExpressionsLength => this.m_Expressions.Length;

        public Typewriter Typewriter => this.m_Typewriter;

        public SpeechSkin OverrideSpeechSkin => this.m_OverrideSpeechSkin;

        public Portrait Portrait => this.m_Portrait;
        
        // GETTERS: -------------------------------------------------------------------------------
        
        public string GetName(Args args) => this.m_Actant.GetName(args);
        public string GetDescription(Args args) => this.m_Actant.GetDescription(args);

        public Expression GetExpressionFromId(IdString id) => this.m_Expressions.FromId(id);
        public Expression GetExpressionFromIndex(int index) => this.m_Expressions.FromIndex(index);
    }
}