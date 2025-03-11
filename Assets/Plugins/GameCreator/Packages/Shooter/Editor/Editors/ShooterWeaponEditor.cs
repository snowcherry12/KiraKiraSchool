using System;
using System.IO;
using System.Reflection;
using GameCreator.Editor.Characters;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using TransformUtils = GameCreator.Runtime.Common.TransformUtils;

namespace GameCreator.Editor.Shooter
{
    [CustomEditor(typeof(ShooterWeapon))]
    public class ShooterWeaponEditor : TWeaponEditor
    {
        private const BindingFlags MEMBER_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;
        
        private const string ASSETS = "Assets/";

        public const string MODEL_PATH = RuntimePaths.PACKAGES + "Shooter/Runtime/Models/Gun.fbx";
        private const string WEAPON_ANIMATOR_PATH = RuntimePaths.PACKAGES + "Shooter/Runtime/Animators/Weapon.overrideController";

        private static int HANDLES_ACTIVE_INDEX = -999;
        private const float HANDLE_SIZE = 0.01f;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private SerializedProperty m_PropertyMagazine;
        private SerializedProperty m_PropertyMuzzle;
        private SerializedProperty m_PropertyCharge;
        private SerializedProperty m_PropertyFire;
        private SerializedProperty m_PropertyProjectile;
        private SerializedProperty m_PropertyAccuracy;
        private SerializedProperty m_PropertyRecoil;
        private SerializedProperty m_PropertyShell;
        private SerializedProperty m_PropertyJam;
        
        private SerializedProperty m_PropertySights;
        private SerializedProperty m_PropertyReloads;
        
        private Button m_ButtonToggleStage;
        
        private Button m_ButtonModel;
        private ObjectField m_ModelField;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override bool HasShieldMember => false;
        protected override bool HasParriedReaction => false;
        protected override bool HasDodge => false;

        // INITIALIZERS: --------------------------------------------------------------------------
        
        private void OnEnable()
        {
            WeaponConfigurationStage.EventOpenStage -= this.RefreshWeaponState;
            WeaponConfigurationStage.EventOpenStage += this.RefreshWeaponState;
            
            WeaponConfigurationStage.EventCloseStage -= this.RefreshWeaponState;
            WeaponConfigurationStage.EventCloseStage += this.RefreshWeaponState;
            
            SceneView.duringSceneGui -= this.SceneGUI;
            SceneView.duringSceneGui += this.SceneGUI;
        }

        private void OnDisable()
        {
            WeaponConfigurationStage.EventOpenStage -= this.RefreshWeaponState;
            WeaponConfigurationStage.EventCloseStage -= this.RefreshWeaponState;
            
            SceneView.duringSceneGui -= this.SceneGUI;
        }
        
        [OnOpenAsset]
        public static bool OpenWeaponExecute(int instanceID, int line)
        {
            ShooterWeapon weapon = EditorUtility.InstanceIDToObject(instanceID) as ShooterWeapon;
            if (weapon == null) return false;

            if (WeaponConfigurationStage.InStage) StageUtility.GoToMainStage();
            Selection.activeObject = weapon;
            
            string skillPath = AssetDatabase.GetAssetPath(weapon);
            WeaponConfigurationStage.EnterStage(skillPath);
            
            return true;
        }

        // INSPECTOR METHODS: ---------------------------------------------------------------------
        
        protected override void CreateGUI(VisualElement root)
        {
            VisualElement content = new VisualElement();
            content.AddToClassList(CLASS_MARGIN_X);
            content.AddToClassList(CLASS_MARGIN_Y);

            root.Add(content);
            
            SerializedProperty state = this.serializedObject.FindProperty("m_State");
            SerializedProperty layer = this.serializedObject.FindProperty("m_Layer");
            
            content.Add(new SpaceSmall());
            content.Add(new PropertyField(state));
            content.Add(new PropertyField(layer));
            
            this.m_ButtonToggleStage = new Button(this.ToggleWeaponMode)
            {
                style = { height = new Length(30f, LengthUnit.Pixel)}
            };

            this.m_ModelField = new ObjectField(string.Empty)
            {
                objectType = typeof(GameObject),
                allowSceneObjects = true,
                style =
                {
                    marginLeft = 0,
                    marginRight = 0,
                    marginTop = 0,
                    marginBottom = 0,
                }
            };

            this.m_ButtonModel = new Button(this.ChangeModel)
            {
                text = "Change Model",
                style =
                {
                    marginRight = 0,
                    marginTop = 0,
                    marginBottom = 0,
                }
            };

            HorizontalBox changeCharacterContent = new HorizontalBox(
                HorizontalBox.FlexMode.FirstGrows,
                this.m_ModelField,
                this.m_ButtonModel
            );
            
            PadBox boxConfigure = new PadBox();
            boxConfigure.Add(this.m_ButtonToggleStage);
            boxConfigure.Add(new SpaceSmaller());
            boxConfigure.Add(changeCharacterContent);
            
            content.Add(new SpaceSmall());
            content.Add(boxConfigure);
            
            this.m_PropertyMagazine = this.serializedObject.FindProperty("m_Magazine");
            this.m_PropertyMuzzle = this.serializedObject.FindProperty("m_Muzzle");
            this.m_PropertyCharge = this.serializedObject.FindProperty("m_Charge");
            this.m_PropertyFire = this.serializedObject.FindProperty("m_Fire");
            this.m_PropertyProjectile = this.serializedObject.FindProperty("m_Projectile");
            this.m_PropertyAccuracy = this.serializedObject.FindProperty("m_Accuracy");
            this.m_PropertyRecoil = this.serializedObject.FindProperty("m_Recoil");
            this.m_PropertyShell = this.serializedObject.FindProperty("m_Shell");
            this.m_PropertyJam = this.serializedObject.FindProperty("m_Jam");
            
            content.Add(new SpaceSmall());
            content.Add(new PropertyField(this.m_PropertyMagazine));
            content.Add(new PropertyField(this.m_PropertyMuzzle));
            content.Add(new PropertyField(this.m_PropertyCharge));
            content.Add(new PropertyField(this.m_PropertyFire));
            content.Add(new PropertyField(this.m_PropertyProjectile));
            content.Add(new PropertyField(this.m_PropertyAccuracy));
            content.Add(new PropertyField(this.m_PropertyRecoil));
            content.Add(new PropertyField(this.m_PropertyShell));
            content.Add(new PropertyField(this.m_PropertyJam));
            
            this.m_PropertySights = this.serializedObject.FindProperty("m_Sights");
            this.m_PropertyReloads = this.serializedObject.FindProperty("m_Reloads");
            
            content.Add(new SpaceSmall());
            content.Add(new LabelTitle("Sights"));
            content.Add(new SpaceSmaller());
            content.Add(new PropertyField(this.m_PropertySights));
            
            content.Add(new SpaceSmall());
            content.Add(new LabelTitle("Reloads"));
            content.Add(new SpaceSmaller());
            content.Add(new PropertyField(this.m_PropertyReloads));
            
            SerializedProperty animations = this.serializedObject.FindProperty("m_Animations");
            content.Add(new SpaceSmall());
            content.Add(new PropertyField(animations));
            
            this.RefreshWeaponState();
        }

        protected override void CreateInFoot(VisualElement foot)
        {
            base.CreateInFoot(foot);
            
            SerializedProperty canShoot = this.serializedObject.FindProperty("m_CanShoot");
            SerializedProperty onShoot = this.serializedObject.FindProperty("m_OnShoot");
            SerializedProperty onReloadStart = this.serializedObject.FindProperty("m_OnStartReload");
            SerializedProperty onReloadFinish = this.serializedObject.FindProperty("m_OnFinishReload");
            SerializedProperty canHit = this.serializedObject.FindProperty("m_CanHit");
            SerializedProperty onHit = this.serializedObject.FindProperty("m_OnHit");

            foot.Add(new SpaceSmall());
            foot.Add(new LabelTitle("Can Shoot:"));
            foot.Add(new SpaceSmaller());
            foot.Add(new PropertyField(canShoot));
            
            foot.Add(new SpaceSmall());
            foot.Add(new LabelTitle("On Shoot:"));
            foot.Add(new SpaceSmaller());
            foot.Add(new PropertyField(onShoot));
            
            foot.Add(new SpaceSmall());
            foot.Add(new LabelTitle("On Start Reload:"));
            foot.Add(new SpaceSmaller());
            foot.Add(new PropertyField(onReloadStart));
            
            foot.Add(new SpaceSmall());
            foot.Add(new LabelTitle("On Finish Reload:"));
            foot.Add(new SpaceSmaller());
            foot.Add(new PropertyField(onReloadFinish));
            
            foot.Add(new SpaceSmall());
            foot.Add(new LabelTitle("Can Hit:"));
            foot.Add(new SpaceSmaller());
            foot.Add(new PropertyField(canHit));
            
            foot.Add(new SpaceSmall());
            foot.Add(new LabelTitle("On Hit:"));
            foot.Add(new SpaceSmaller());
            foot.Add(new PropertyField(onHit));
        }

        // SCENE GUI: -----------------------------------------------------------------------------
        
        private void SceneGUI(SceneView sceneView)
        {
            if (WeaponConfigurationStage.InStage == false) return;

            EditorGUI.BeginChangeCheck();
            
            SceneHandle(this.m_PropertyMuzzle, "m_Position", "m_Rotation", -1, "Muzzle", sceneView);
            SceneHandle(this.m_PropertyShell, "m_Position", "m_Rotation", -2, "Shell", sceneView);
            
            SerializedProperty sights = this.m_PropertySights.FindPropertyRelative("m_Sights");
            
            for (int i = 0; i < sights.arraySize; ++i)
            {
                SerializedProperty sightItem = sights.GetArrayElementAtIndex(i);
                if (sightItem.FindPropertyRelative("m_ScopeThrough").boolValue == false) continue;
                
                string title = sightItem
                    .FindPropertyRelative("m_Id")
                    .FindPropertyRelative(IdStringDrawer.NAME_STRING)
                    .stringValue;

                SceneHandle(sightItem, "m_ScopePosition", "m_ScopeRotation", i, title, sceneView);
            }

            if (EditorGUI.EndChangeCheck())
            {
                this.serializedObject.ApplyModifiedProperties();
            }
        }

        private static void SceneHandle(
            SerializedProperty item,
            string positionName,
            string rotationName,
            int i,
            string title,
            SceneView sceneView)
        {
            if (item == null) return;
            
            Vector3 modelPosition = Vector3.zero;
            Quaternion modelRotation = Quaternion.identity;
            Vector3 modelScale = Vector3.one;

            if (WeaponConfigurationStage.Stage.Model != null)
            {
                modelPosition = WeaponConfigurationStage.Stage.Model.transform.position;
                modelRotation = WeaponConfigurationStage.Stage.Model.transform.rotation;
                modelScale = WeaponConfigurationStage.Stage.Model.transform.lossyScale;
            }

            SerializedProperty position = string.IsNullOrEmpty(positionName) == false
                ? item.FindPropertyRelative(positionName)
                : null;
            
            SerializedProperty rotation = string.IsNullOrEmpty(rotationName) == false
                ? item.FindPropertyRelative(rotationName)
                : null;

            Vector3 positionHandle = position != null
                ? TransformUtils.TransformPoint(
                    position.vector3Value,
                    modelPosition,
                    modelRotation,
                    modelScale
                ) : default;

            Quaternion rotationHandle = rotation != null
                ? TransformUtils.TransformRotation(
                    Quaternion.Euler(rotation.vector3Value),
                    modelPosition,
                    modelRotation,
                    modelScale
                ) : default;
            
            if (HANDLES_ACTIVE_INDEX != i)
            {
                bool selection = Handles.Button(
                    positionHandle, 
                    modelRotation, 
                    HANDLE_SIZE, 
                    HANDLE_SIZE * 2f,
                    Handles.SphereHandleCap
                );

                if (selection)
                {
                    if (Tools.current != Tool.Move &&
                        Tools.current != Tool.Rotate &&
                        Tools.current != Tool.Transform)
                    {
                        Tools.current = Tool.Transform;   
                    }
                    
                    HANDLES_ACTIVE_INDEX = i;
                }
            }
            
            if (HANDLES_ACTIVE_INDEX == i)
            {
                switch (Tools.current)
                {
                    case Tool.Move:
                        if (position == null) break;
                        positionHandle = Handles.PositionHandle(
                            positionHandle,
                            Tools.pivotRotation switch
                            {
                                PivotRotation.Local => rotation != null ? rotationHandle : modelRotation,
                                PivotRotation.Global => Quaternion.LookRotation(Vector3.forward),
                                _ => throw new ArgumentOutOfRangeException()
                            }
                        );
                        break;
                    
                    case Tool.Rotate:
                        if (rotation == null) break;
                        rotationHandle = Handles.RotationHandle(
                            rotationHandle,
                            position != null ? positionHandle : modelPosition
                        );
                        break;
                    
                    case Tool.Transform:
                        if (position != null && rotation == null)
                        {
                            positionHandle = Handles.PositionHandle(
                                positionHandle,
                                Tools.pivotRotation switch
                                {
                                    PivotRotation.Local => modelRotation,
                                    PivotRotation.Global => Quaternion.LookRotation(Vector3.forward),
                                    _ => throw new ArgumentOutOfRangeException()
                                }
                            );
                        }
                        if (position == null && rotation != null)
                        {
                            rotationHandle = Handles.RotationHandle(
                                rotationHandle,
                                modelPosition
                            );
                        }
                        if (position != null && rotation != null)
                        {
                            Handles.TransformHandle(
                                ref positionHandle,
                                ref rotationHandle
                            );
                        }
                        break;

                    case Tool.View:
                    case Tool.Scale:
                    case Tool.Rect:
                    case Tool.Custom:
                    case Tool.None:
                    default:
                        Handles.Label(
                            positionHandle + sceneView.rotation * Vector3.right * 0.025f,
                            title
                        );
                        break;
                }

                if (position != null)
                {
                    position.vector3Value = TransformUtils.InverseTransformPoint(
                        positionHandle,
                        modelPosition,
                        modelRotation,
                        modelScale
                    );
                }

                if (rotation != null)
                {
                    rotation.vector3Value = TransformUtils.InverseTransformRotation(
                        rotationHandle,
                        modelPosition,
                        modelRotation,
                        modelScale
                    ).eulerAngles;
                }
            }
            else
            {
                Handles.Label(
                    positionHandle + sceneView.rotation * Vector3.right * 0.025f,
                    title
                );
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void ChangeModel()
        {
            GameObject character = this.m_ModelField.value as GameObject;
            WeaponConfigurationStage.ChangeModel(character);
        }

        private void ToggleWeaponMode()
        {
            if (WeaponConfigurationStage.InStage)
            {
                StageUtility.GoToMainStage();
                return;
            }

            ShooterWeapon shooterWeapon = this.target as ShooterWeapon;
            if (shooterWeapon == null) return;

            string path = AssetDatabase.GetAssetPath(shooterWeapon);
            WeaponConfigurationStage.EnterStage(path);
        }

        private void RefreshWeaponState()
        {
            if (this.m_ButtonToggleStage == null) return;
            
            bool isWeaponMode = WeaponConfigurationStage.InStage;
            this.m_ButtonToggleStage.text = isWeaponMode
                ? "Close Weapon Mode" 
                : "Enter Weapon Mode";
            
            Color borderColor = isWeaponMode
                ? ColorTheme.Get(ColorTheme.Type.Green)
                : ColorTheme.Get(ColorTheme.Type.Dark);
            
            this.m_ButtonToggleStage.style.borderTopColor = borderColor;
            this.m_ButtonToggleStage.style.borderBottomColor = borderColor;
            this.m_ButtonToggleStage.style.borderLeftColor = borderColor;
            this.m_ButtonToggleStage.style.borderRightColor = borderColor;
            
            this.m_ButtonToggleStage.style.color = isWeaponMode
                ? ColorTheme.Get(ColorTheme.Type.Green)
                : ColorTheme.Get(ColorTheme.Type.TextNormal);
            
            this.m_ButtonModel.SetEnabled(isWeaponMode);
            this.m_ModelField.SetEnabled(isWeaponMode);
            
            if (isWeaponMode)
            {
                this.m_ModelField.value = WeaponConfigurationStage.ModelReference;
            }
        }
        
        // CREATE STATE: --------------------------------------------------------------------------
        
        [MenuItem("Assets/Create/Game Creator/Shooter/Weapon", false, 50)]
        internal static void CreateFromMenuItem()
        {
            ShooterWeapon weapon = CreateInstance<ShooterWeapon>();

            string selection = Selection.activeObject != null
                ? AssetDatabase.GetAssetPath(Selection.activeObject)
                : ASSETS;

            string directory = File.Exists(PathUtils.PathForOS(selection)) 
                ? PathUtils.PathToUnix(Path.GetDirectoryName(selection)) 
                : selection;

            string path = AssetDatabase.GenerateUniqueAssetPath(
                PathUtils.Combine(directory ?? ASSETS, "Weapon (Shooter).asset")
            );
            
            DirectoryUtils.RequireFilepath(path);
            AssetDatabase.CreateAsset(weapon, path);
            AssetDatabase.SaveAssets();
            
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = weapon;
            
            AnimatorOverrideController controller = Instantiate(
                AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(
                    WEAPON_ANIMATOR_PATH
                )
            );

            controller.name = Path.GetFileNameWithoutExtension(WEAPON_ANIMATOR_PATH); 
            controller.hideFlags = HideFlags.HideInHierarchy;
            
            AssetDatabase.AddObjectToAsset(controller, weapon);
            typeof(ShooterWeapon).GetField("m_Controller", MEMBER_FLAGS)?.SetValue(weapon, controller);

            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(weapon));
        }
    }
}