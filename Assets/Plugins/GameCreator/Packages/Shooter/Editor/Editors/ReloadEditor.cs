using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomEditor(typeof(Reload))]
    public class ReloadEditor : UnityEditor.Editor
    {
        private const string ERR_ANIM = "A Reload requires an Animation Clip";
        private const int BUTTON_WIDTH = 150;

        // MEMBERS: -------------------------------------------------------------------------------

        private Reload m_Reload;
        private VisualElement m_Root;
        
        private Button m_ButtonToggleStage;
        
        private Button m_ButtonCharacter;
        private ObjectField m_CharacterField;
        
        private Button m_ButtonWeapon;
        private ObjectField m_WeaponField;

        private ReloadSequenceTool m_SequenceTool;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnEnable()
        {
            this.m_Reload = this.target as Reload;
            
            ReloadConfigurationStage.EventOpenStage -= this.RefreshReloadState;
            ReloadConfigurationStage.EventOpenStage += this.RefreshReloadState;
            
            ReloadConfigurationStage.EventCloseStage -= this.RefreshReloadState;
            ReloadConfigurationStage.EventCloseStage += this.RefreshReloadState;
        }

        private void OnDisable()
        {
            ReloadConfigurationStage.EventOpenStage -= this.RefreshReloadState;
            ReloadConfigurationStage.EventCloseStage -= this.RefreshReloadState;
        }

        [OnOpenAsset]
        public static bool OpenSkillExecute(int instanceID, int line)
        {
            Reload reload = EditorUtility.InstanceIDToObject(instanceID) as Reload;
            if (reload == null) return false;

            if (ReloadConfigurationStage.InStage) StageUtility.GoToMainStage();
            Selection.activeObject = reload;
            
            string skillPath = AssetDatabase.GetAssetPath(reload);
            ReloadConfigurationStage.EnterStage(skillPath);
            
            return true;
        }

        // INSPECTOR METHODS: ---------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            
            StyleSheet[] styleSheets = StyleSheetUtils.Load();
            foreach (StyleSheet styleSheet in styleSheets) this.m_Root.styleSheets.Add(styleSheet);

            SerializedProperty title = this.serializedObject.FindProperty("m_Title");
            SerializedProperty description = this.serializedObject.FindProperty("m_Description");
            
            this.m_Root.Add(new PropertyField(title));
            this.m_Root.Add(new PropertyField(description));
            
            SerializedProperty icon = this.serializedObject.FindProperty("m_Icon");
            SerializedProperty color = this.serializedObject.FindProperty("m_Color");
            
            this.m_Root.Add(new PropertyField(icon));
            this.m_Root.Add(new PropertyField(color));

            SerializedProperty animation = this.serializedObject.FindProperty("m_Animation");
            SerializedProperty mask = this.serializedObject.FindProperty("m_Mask");
            
            SerializedProperty transitionIn = this.serializedObject.FindProperty("m_TransitionIn");
            SerializedProperty transitionOut = this.serializedObject.FindProperty("m_TransitionOut");
            
            ErrorMessage animationError = new ErrorMessage(ERR_ANIM);
            PropertyField animationField = new PropertyField(animation);
            PropertyField maskField = new PropertyField(mask);

            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(animationError);
            this.m_Root.Add(animationField);
            this.m_Root.Add(maskField);

            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(transitionIn));
            this.m_Root.Add(new PropertyField(transitionOut));
            
            this.m_ButtonToggleStage = new Button(this.ToggleReloadMode)
            {
                style = { height = new Length(30f, LengthUnit.Pixel)}
            };

            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(this.m_ButtonToggleStage);
            
            PadBox sequenceContent = new PadBox();
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(sequenceContent);
            
            this.m_CharacterField = new ObjectField(string.Empty)
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

            this.m_ButtonCharacter = new Button(this.ChangeCharacter)
            {
                text = "Change Character",
                style =
                {
                    marginRight = 0,
                    marginTop = 0,
                    marginBottom = 0,
                    width = BUTTON_WIDTH,
                }
            };
            
            HorizontalBox changeCharacterContent = new HorizontalBox(
                HorizontalBox.FlexMode.FirstGrows,
                this.m_CharacterField,
                this.m_ButtonCharacter
            );
            
            sequenceContent.Add(changeCharacterContent);
            
            this.m_WeaponField = new ObjectField(string.Empty)
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

            this.m_ButtonWeapon = new Button(this.ChangeWeapon)
            {
                text = "Change Weapon",
                style =
                {
                    marginRight = 0,
                    marginTop = 0,
                    marginBottom = 0,
                    width = BUTTON_WIDTH,
                }
            };
            
            HorizontalBox changeWeaponContent = new HorizontalBox(
                HorizontalBox.FlexMode.FirstGrows,
                this.m_WeaponField,
                this.m_ButtonWeapon
            );

            GameObject character = this.m_Reload != null
                ? AssetDatabase.LoadAssetAtPath<GameObject>(this.m_Reload.EditorModelPath)
                : null;
            
            GameObject weapon = this.m_Reload != null
                ? AssetDatabase.LoadAssetAtPath<GameObject>(this.m_Reload.EditorWeaponPath)
                : null;
                    
            this.m_CharacterField.value = character;
            this.m_WeaponField.value = weapon;
            
            sequenceContent.Add(new SpaceSmaller());
            sequenceContent.Add(changeWeaponContent);

            SerializedProperty sequence = this.serializedObject
                .FindProperty("m_ReloadSequence")
                .FindPropertyRelative(RunReloadSequenceDrawer.NAME_SEQUENCE);

            this.m_SequenceTool = new ReloadSequenceTool(sequence)
            {
                AnimationClip = animation.objectReferenceValue as AnimationClip
            };

            sequenceContent.Add(new SpaceSmall());
            sequenceContent.Add(this.m_SequenceTool);
            
            animationField.RegisterValueChangeCallback(changeEvent =>
            {
                Object newValue = changeEvent.changedProperty.objectReferenceValue;
                animationError.style.display = newValue == null
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
                
                this.m_SequenceTool.AnimationClip = newValue as AnimationClip;
            });
            
            animationError.style.display = animation.objectReferenceValue == null
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            SerializedProperty speed = this.serializedObject.FindProperty("m_Speed");
            SerializedProperty discard = this.serializedObject.FindProperty("m_DiscardMagazineAmmo");
            SerializedProperty reload = this.serializedObject.FindProperty("m_Reload");
            SerializedProperty reloadAmount = this.serializedObject.FindProperty("m_ReloadAmount");

            PropertyField fieldReload = new PropertyField(reload);
            PropertyField fieldReloadAmount = new PropertyField(reloadAmount, "Amount");
            
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(speed));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new PropertyField(discard));
            this.m_Root.Add(fieldReload);
            this.m_Root.Add(fieldReloadAmount);
            
            fieldReload.RegisterValueChangeCallback(changeEvent =>
            {
                fieldReloadAmount.style.display = changeEvent.changedProperty.enumValueIndex != 0
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });
            
            fieldReloadAmount.style.display = reload.enumValueIndex != 0
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            SerializedProperty onStart = this.serializedObject.FindProperty("m_OnStart");
            SerializedProperty onFinish = this.serializedObject.FindProperty("m_OnFinish");

            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new LabelTitle("On Start:"));
            this.m_Root.Add(new SpaceSmaller());
            this.m_Root.Add(new PropertyField(onStart));
            this.m_Root.Add(new SpaceSmall());
            this.m_Root.Add(new LabelTitle("On Finish:"));
            this.m_Root.Add(new SpaceSmaller());
            this.m_Root.Add(new PropertyField(onFinish));
            
            this.RefreshReloadState();
            
            return this.m_Root;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void ChangeCharacter()
        {
            GameObject character = this.m_CharacterField.value as GameObject;
            ReloadConfigurationStage.ChangeCharacter(character);
            
            if (ReloadConfigurationStage.InStage)
            {
                this.m_SequenceTool.Target = ReloadConfigurationStage.Stage.Animator != null
                    ? ReloadConfigurationStage.Stage.Animator.gameObject
                    : null;
            }
        }
        
        private void ChangeWeapon()
        {
            GameObject weapon = this.m_WeaponField.value as GameObject;
            ReloadConfigurationStage.ChangeWeapon(weapon);
            
            if (ReloadConfigurationStage.InStage)
            {
                this.m_SequenceTool.Target = ReloadConfigurationStage.Stage.Animator != null
                    ? ReloadConfigurationStage.Stage.Animator.gameObject
                    : null;
            }
        }

        private void ToggleReloadMode()
        {
            if (ReloadConfigurationStage.InStage)
            {
                StageUtility.GoToMainStage();
                this.RefreshReloadState();
                
                this.m_SequenceTool.DisablePreview();
                return;
            }

            Reload reload = this.target as Reload;
            if (reload == null) return;

            string path = AssetDatabase.GetAssetPath(reload);
            ReloadConfigurationStage.EnterStage(path);
            
            this.m_SequenceTool.Target = ReloadConfigurationStage.Stage.Animator != null
                ? ReloadConfigurationStage.Stage.Animator.gameObject
                : null; 
        }
        
        private void RefreshReloadState()
        {
            if (this.m_ButtonToggleStage == null) return;
            
            bool isReloadMode = ReloadConfigurationStage.InStage;
            this.m_ButtonToggleStage.text = isReloadMode
                ? "Close Reload Mode" 
                : "Enter Reload Mode";

            Color borderColor = isReloadMode
                ? ColorTheme.Get(ColorTheme.Type.Green)
                : ColorTheme.Get(ColorTheme.Type.Dark);
            
            this.m_ButtonToggleStage.style.borderTopColor = borderColor;
            this.m_ButtonToggleStage.style.borderBottomColor = borderColor;
            this.m_ButtonToggleStage.style.borderLeftColor = borderColor;
            this.m_ButtonToggleStage.style.borderRightColor = borderColor;

            this.m_ButtonToggleStage.style.color = isReloadMode
                ? ColorTheme.Get(ColorTheme.Type.Green)
                : ColorTheme.Get(ColorTheme.Type.TextNormal);

            this.m_ButtonCharacter.SetEnabled(isReloadMode);
            this.m_CharacterField.SetEnabled(isReloadMode);
            this.m_ButtonWeapon.SetEnabled(isReloadMode);
            this.m_WeaponField.SetEnabled(isReloadMode);
            this.m_SequenceTool.IsEnabled = isReloadMode;
            
            if (isReloadMode)
            {
                this.m_CharacterField.value = ReloadConfigurationStage.CharacterReference;
                this.m_WeaponField.value = ReloadConfigurationStage.WeaponReference;
                
                this.m_SequenceTool.Target = ReloadConfigurationStage.Stage.Animator != null
                    ? ReloadConfigurationStage.Stage.Animator.gameObject
                    : null; 
            }
        }
    }
}