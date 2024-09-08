using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Stats
{
    [CreateAssetMenu(
        fileName = "Status Effect", 
        menuName = "Game Creator/Stats/Status Effect",
        order    = 50
    )]
    
    [Icon(EditorPaths.PACKAGES + "Stats/Editor/Gizmos/GizmoStatusEffects.png")]
    
    public class StatusEffect : ScriptableObject
    {
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode()
        {
            LastAdded = null;
            LastRemoved = null;
        }
        
        #endif
        
        public static StatusEffect LastAdded { get; internal set; }
        public static StatusEffect LastRemoved { get; internal set; }
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private StatusEffectData m_Data = new StatusEffectData();
        [SerializeField] private StatusEffectInfo m_Info = new StatusEffectInfo();

        [SerializeField] private RunInstructionsList m_OnStart = new RunInstructionsList(
            new InstructionCommonDebugComment("Executed right after this Status Effect is applied")
        );
        
        [SerializeField] private RunInstructionsList m_OnEnd = new RunInstructionsList(
            new InstructionCommonDebugComment("Executed right after this Status Effect finishes")
        );
        
        [SerializeField] private RunInstructionsList m_WhileActive = new RunInstructionsList(
            new InstructionCommonDebugComment("Executed over and over again while this Status Effect lasts")
        );

        [SerializeField] private SaveUniqueID m_ID = new SaveUniqueID(false, "my-unique-id");

        // PROPERTIES: ----------------------------------------------------------------------------

        public IdString ID => this.m_ID.Get;
        
        public Color Color => this.m_Info.Color;
        public StatusEffectType Type => this.m_Data.Type;

        public bool Save => this.m_ID.SaveValue;
        public bool HasDuration => this.m_Data.HasDuration;
        public bool IsHidden => this.m_Data.IsHidden;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public string GetAcronym(Args args) => this.m_Info.m_Acronym.Get(args);
        public string GetName(Args args) => this.m_Info.m_Name.Get(args);
        public string GetDescription(Args args) => this.m_Info.m_Description.Get(args);
        public Sprite GetIcon(Args args) => this.m_Info.GetIcon(args);

        public float GetDuration(Args args) => (float) this.m_Data.GetDuration(args);
        public int GetMaxStack(Args args) => this.m_Data.GetMaxStack(args);

        public Task RunOnStart(Args args, RunnerConfig config) => this.m_OnStart.Run(args, config);
        public Task RunOnEnd(Args args, RunnerConfig config) => this.m_OnEnd.Run(args, config);
        public Task RunWhileActive(Args args, RunnerConfig config) => this.m_WhileActive.Run(args, config);
    }
}