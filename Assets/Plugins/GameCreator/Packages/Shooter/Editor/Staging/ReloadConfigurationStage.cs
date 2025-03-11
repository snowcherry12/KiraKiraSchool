using System;
using GameCreator.Editor.Characters;
using GameCreator.Editor.Core;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TransformUtils = GameCreator.Runtime.Common.TransformUtils;

namespace GameCreator.Editor.Shooter
{
    public class ReloadConfigurationStage : TPreviewSceneStage<ReloadConfigurationStage>
    {
        private const string HEADER_TITLE = "Reload Configuration";
        private const string HEADER_ICON = RuntimePaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoReload.png";

        public static GameObject CharacterReference { get; private set; }
        public static GameObject WeaponReference { get; private set; }
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private GameObject m_Character;
        [NonSerialized] private GameObject m_Weapon;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Title => $" {HEADER_TITLE}";
        protected override string Icon => HEADER_ICON;

        public Reload Reload => this.Asset as Reload;
        
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

            if (Stage.m_Weapon != null)
            {
                Stage.m_Weapon.transform.SetParent(null);
            }
            
            CharacterReference = reference;
            Stage.Reload.EditorModelPath = AssetDatabase.GetAssetPath(reference);
            GameObject character = GetCharacterTarget();

            if (Stage.m_Character != null) DestroyImmediate(Stage.m_Character);
            Stage.m_Character = character;
            
            StagingGizmos.Bind(Stage.m_Character, Stage.Reload);
            StageUtility.PlaceGameObjectInCurrentStage(Stage.m_Character);
            
            RefreshWeaponOnCharacter();
        }
        
        public static void ChangeWeapon(GameObject reference)
        {
            if (!InStage) return;

            WeaponReference = reference;
            Stage.Reload.EditorWeaponPath = AssetDatabase.GetAssetPath(reference);
            GameObject weapon = GetWeaponTarget();
            
            if (Stage.m_Weapon != null) DestroyImmediate(Stage.m_Weapon);
            Stage.m_Weapon = weapon;
            
            StageUtility.PlaceGameObjectInCurrentStage(Stage.m_Weapon);
            RefreshWeaponOnCharacter();
        }

        // INITIALIZE METHODS: --------------------------------------------------------------------

        public override void AfterStageSetup()
        {
            base.AfterStageSetup();
            
            Stage.m_Character = GetCharacterTarget();
            if (Stage.m_Character == null) return;
            
            StagingGizmos.Bind(Stage.m_Character, Stage.Reload);
            StageUtility.PlaceGameObjectInCurrentStage(Stage.m_Character);

            Stage.m_Weapon = GetWeaponTarget();
            if (Stage.m_Weapon == null) return;
            
            StageUtility.PlaceGameObjectInCurrentStage(Stage.m_Weapon);
            RefreshWeaponOnCharacter();
        }

        protected override bool OnOpenStage()
        {
            if (!base.OnOpenStage()) return false;
            
            EventOpenStage?.Invoke();
            return true;
        }

        protected override void OnCloseStage()
        {
            if (this.m_Character != null && this.m_Weapon != null)
            {
                Transform bone = this.m_Weapon.transform.parent;
                if (bone != null)
                {
                    string bonePath = TransformUtils.GetHierarchyPath(bone, this.m_Character.transform);
                    
                    this.Reload.EditorWeaponBone = bonePath;
                    this.Reload.EditorWeaponLocalPosition = this.m_Weapon.transform.localPosition;
                    this.Reload.EditorWeaponLocalRotation = this.m_Weapon.transform.localRotation;
                }
            }
            
            base.OnCloseStage();
            EventCloseStage?.Invoke();
        }

        // PRIVATE STATIC METHODS: ----------------------------------------------------------------
        
        private static GameObject GetCharacterTarget()
        {
            if (Stage == null || Stage.Reload == null) return null;
            
            GameObject source = AssetDatabase.LoadAssetAtPath<GameObject>(Stage.Reload.EditorModelPath);
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
        
        private static GameObject GetWeaponTarget()
        {
            if (Stage == null || Stage.Reload == null) return null;
            
            GameObject source = AssetDatabase.LoadAssetAtPath<GameObject>(Stage.Reload.EditorWeaponPath);
            if (source == null)
            {
                source = WeaponReference == null
                    ? AssetDatabase.LoadAssetAtPath<GameObject>(ShooterWeaponEditor.MODEL_PATH)
                    : WeaponReference;
            }

            if (source == null) return null;
            
            GameObject target = Instantiate(source);
            if (target == null) return null;
            
            target.name = source.name;
            return target;
        }
        
        private static void RefreshWeaponOnCharacter()
        {
            if (Stage.m_Character != null && Stage.m_Weapon != null)
            {
                Transform bone = Stage.m_Character.transform.Find(Stage.Reload.EditorWeaponBone);
                Vector3 position = Stage.Reload.EditorWeaponLocalPosition;
                Quaternion rotation = Stage.Reload.EditorWeaponLocalRotation;
                
                if (bone != null)
                {
                    Stage.m_Weapon.transform.SetParent(bone);
                    Stage.m_Weapon.transform.localPosition = position;
                    Stage.m_Weapon.transform.localRotation = rotation;
                }
            }
        }
    }
}