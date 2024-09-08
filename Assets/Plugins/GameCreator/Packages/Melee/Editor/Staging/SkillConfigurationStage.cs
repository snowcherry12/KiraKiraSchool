using System;
using GameCreator.Editor.Characters;
using GameCreator.Editor.Core;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GameCreator.Editor.Melee
{
    public class SkillConfigurationStage : TPreviewSceneStage<SkillConfigurationStage>
    {
        private const string HEADER_TITLE = "Skill Configuration";
        private const string HEADER_ICON = RuntimePaths.PACKAGES + "Melee/Editor/Gizmos/GizmoSkill.png";

        public static GameObject CharacterReference { get; private set; }
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private GameObject m_Character;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Title => HEADER_TITLE;
        protected override string Icon => HEADER_ICON;

        public Skill Skill => this.Asset as Skill;
        
        public Animator Animator => this.m_Character != null
            ? this.m_Character.GetComponent<Animator>()
            : null;

        protected override GameObject FocusOn => this.m_Character;

        // EVENTS: --------------------------------------------------------------------------------

        public static Action EventOpenStage;
        public static Action EventCloseStage;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static void ChangeCharacter(GameObject reference)
        {
            if (!InStage) return;

            CharacterReference = reference;
            Stage.Skill.EditorModelPath = AssetDatabase.GetAssetPath(reference);
            GameObject character = GetTarget();

            if (Stage.m_Character != null) DestroyImmediate(Stage.m_Character);
            Stage.m_Character = character;
            
            StagingGizmos.Bind(Stage.m_Character, Stage.Skill);
            StageUtility.PlaceGameObjectInCurrentStage(Stage.m_Character);
        }

        // INITIALIZE METHODS: --------------------------------------------------------------------

        public override void AfterStageSetup()
        {
            base.AfterStageSetup();
            
            Stage.m_Character = GetTarget();
            if (Stage.m_Character == null) return;
            
            StagingGizmos.Bind(Stage.m_Character, Stage.Skill);
            StageUtility.PlaceGameObjectInCurrentStage(Stage.m_Character);
        }

        protected override bool OnOpenStage()
        {
            if (!base.OnOpenStage()) return false;
            
            EventOpenStage?.Invoke();
            return true;
        }

        protected override void OnCloseStage()
        {
            base.OnCloseStage();
            EventCloseStage?.Invoke();
        }

        // PRIVATE STATIC METHODS: ----------------------------------------------------------------

        private static GameObject GetTarget()
        {
            if (Stage == null || Stage.Skill == null) return null;
            
            GameObject source = AssetDatabase.LoadAssetAtPath<GameObject>(Stage.Skill.EditorModelPath);
            if (source == null)
            {
                source = CharacterReference == null
                    ? AssetDatabase.LoadAssetAtPath<GameObject>(CharacterEditor.MODEL_PATH)
                    : CharacterReference;
            }

            if (source == null) return null;
            GameObject target = Instantiate(source);

            if (target == null) return null;
            if (target.TryGetComponent(out Character character))
            {
                if (character.Animim.Animator != null)
                {
                    GameObject child = Instantiate(character.Animim.Animator.gameObject);
                    
                    DestroyImmediate(target);
                    target = child;
                }
            }

            if (target == null) return null;
            target.name = source.name;

            return target;
        }
    }
}