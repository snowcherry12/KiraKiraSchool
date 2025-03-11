using System;
using GameCreator.Editor.Core;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GameCreator.Editor.Shooter
{
    public class WeaponConfigurationStage : TPreviewSceneStage<WeaponConfigurationStage>
    {
        private const string HEADER_TITLE = "Weapon Configuration";
        private const string HEADER_ICON = RuntimePaths.PACKAGES + "Shooter/Editor/Gizmos/GizmoShooterWeapon.png";

        public static GameObject ModelReference { get; private set; }
        
        // MEMBERS: -------------------------------------------------------------------------------

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string Title => $" {HEADER_TITLE}";
        protected override string Icon => HEADER_ICON;

        public ShooterWeapon Weapon => this.Asset as ShooterWeapon;
        
        [field: NonSerialized] public GameObject Model { get; private set; }

        protected override GameObject FocusOn => this.Model;

        // EVENTS: --------------------------------------------------------------------------------

        public static Action EventOpenStage;
        public static Action EventCloseStage;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static void ChangeModel(GameObject reference)
        {
            if (!InStage) return;

            ModelReference = reference;
            Stage.Weapon.EditorModelPath = AssetDatabase.GetAssetPath(reference);
            GameObject model = GetTarget();

            if (Stage.Model != null) DestroyImmediate(Stage.Model);
            Stage.Model = model;
            
            SceneVisibilityManager.instance.DisablePicking(Stage.Model, true);
            
            StagingGizmos.Bind(Stage.Model, Stage.Weapon);
            StageUtility.PlaceGameObjectInCurrentStage(Stage.Model);
        }

        // INITIALIZE METHODS: --------------------------------------------------------------------

        public override void AfterStageSetup()
        {
            base.AfterStageSetup();
            
            Stage.Model = GetTarget();
            if (Stage.Model == null) return;
            
            SceneVisibilityManager.instance.DisablePicking(Stage.Model, true);
            
            StagingGizmos.Bind(Stage.Model, Stage.Weapon);
            StageUtility.PlaceGameObjectInCurrentStage(Stage.Model);
            
            Selection.activeObject = Stage.Weapon != null ? Stage.Weapon : Stage.Model;
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
            if (Stage == null || Stage.Weapon == null) return null;

            GameObject source = AssetDatabase.LoadAssetAtPath<GameObject>(Stage.Weapon.EditorModelPath);
            if (source == null)
            {
                source = ModelReference == null
                    ? AssetDatabase.LoadAssetAtPath<GameObject>(ShooterWeaponEditor.MODEL_PATH)
                    : ModelReference;
            }

            if (source == null) return null;
            GameObject target = Instantiate(source);

            if (target == null) return null;
            target.name = source.name;
            
            return target;
        }
    }
}