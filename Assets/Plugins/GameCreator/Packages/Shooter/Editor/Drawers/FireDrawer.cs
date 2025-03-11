using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Shooter;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Shooter
{
    [CustomPropertyDrawer(typeof(Fire))]
    public class FireDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Fire";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty projectiles = property.FindPropertyRelative("m_ProjectilesPerShot");
            SerializedProperty cartridges = property.FindPropertyRelative("m_CartridgesPerShot");
            SerializedProperty mode = property.FindPropertyRelative("m_Mode");
            SerializedProperty autoLoading = property.FindPropertyRelative("m_AutoLoading");
            SerializedProperty autoLoadDuration = property.FindPropertyRelative("m_AutoLoadDuration");
            SerializedProperty fireRate = property.FindPropertyRelative("m_FireRate");
            SerializedProperty burst = property.FindPropertyRelative("m_Burst");
            SerializedProperty minCharge = property.FindPropertyRelative("m_MinChargeTime");
            SerializedProperty maxCharge = property.FindPropertyRelative("m_MaxChargeTime");
            SerializedProperty autoRelease = property.FindPropertyRelative("m_AutoRelease");
            SerializedProperty duration = property.FindPropertyRelative("m_Duration");

            PropertyField fieldProjectiles = new PropertyField(projectiles);
            PropertyField fieldCartridges = new PropertyField(cartridges);
            PropertyField fieldMode = new PropertyField(mode);
            PropertyField fieldAutoLoading = new PropertyField(autoLoading);
            PropertyField fieldAutoLoadDuration = new PropertyField(autoLoadDuration);
            PropertyField fieldFireRate = new PropertyField(fireRate);
            PropertyField fieldBurst = new PropertyField(burst);
            PropertyField fieldMinCharge = new PropertyField(minCharge);
            PropertyField fieldMaxCharge = new PropertyField(maxCharge);
            PropertyField fieldAutoRelease = new PropertyField(autoRelease);
            PropertyField fieldDuration = new PropertyField(duration);
            
            VisualElement fieldsCharge = new VisualElement();
            
            fieldsCharge.Add(fieldMinCharge);
            fieldsCharge.Add(new SpaceSmaller());
            fieldsCharge.Add(fieldMaxCharge);
            fieldsCharge.Add(new SpaceSmall());
            fieldsCharge.Add(fieldAutoRelease);
            fieldsCharge.Add(fieldDuration);
            
            container.Add(fieldProjectiles);
            container.Add(fieldCartridges);
            container.Add(fieldMode);
            container.Add(new SpaceSmall());
            container.Add(fieldAutoLoading);
            container.Add(fieldAutoLoadDuration);
            container.Add(fieldFireRate);
            container.Add(fieldBurst);
            container.Add(fieldsCharge);
            
            fieldAutoLoading.RegisterValueChangeCallback(changeEvent =>
            {
                fieldAutoLoadDuration.style.display = changeEvent.changedProperty.enumValueIndex != 0
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });
            
            fieldMode.RegisterValueChangeCallback(changeEvent =>
            {
                switch ((ShootMode) changeEvent.changedProperty.enumValueIndex)
                {
                    case ShootMode.Single:
                        fieldAutoLoading.style.display = DisplayStyle.None;
                        fieldAutoLoadDuration.style.display = DisplayStyle.None;
                        fieldFireRate.style.display = DisplayStyle.Flex;
                        fieldBurst.style.display = DisplayStyle.None;
                        fieldsCharge.style.display = DisplayStyle.None;
                        break;
                    
                    case ShootMode.Burst:
                        fieldAutoLoading.style.display = DisplayStyle.None;
                        fieldAutoLoadDuration.style.display = DisplayStyle.None;
                        fieldFireRate.style.display = DisplayStyle.Flex;
                        fieldBurst.style.display = DisplayStyle.Flex;
                        fieldsCharge.style.display = DisplayStyle.None;
                        break;
                    
                    case ShootMode.FullAuto:
                        fieldAutoLoading.style.display = DisplayStyle.Flex;
                        fieldAutoLoadDuration.style.display = autoLoading.enumValueIndex != 0
                            ? DisplayStyle.Flex
                            : DisplayStyle.None;
                        fieldFireRate.style.display = DisplayStyle.Flex;
                        fieldBurst.style.display = DisplayStyle.None;
                        fieldsCharge.style.display = DisplayStyle.None;
                        break;
                    
                    case ShootMode.Charge:
                        fieldAutoLoading.style.display = DisplayStyle.None;
                        fieldAutoLoadDuration.style.display = DisplayStyle.None;
                        fieldFireRate.style.display = DisplayStyle.None;
                        fieldBurst.style.display = DisplayStyle.None;
                        fieldsCharge.style.display = DisplayStyle.Flex;
                        break;
                    
                    default: throw new ArgumentOutOfRangeException();
                }
            });
            
            switch ((ShootMode) mode.enumValueIndex)
            {
                case ShootMode.Single:
                    fieldAutoLoading.style.display = DisplayStyle.None;
                    fieldAutoLoadDuration.style.display = DisplayStyle.None;
                    fieldFireRate.style.display = DisplayStyle.Flex;
                    fieldBurst.style.display = DisplayStyle.None;
                    fieldsCharge.style.display = DisplayStyle.None;
                    break;
                    
                case ShootMode.Burst:
                    fieldAutoLoading.style.display = DisplayStyle.None;
                    fieldAutoLoadDuration.style.display = DisplayStyle.None;
                    fieldFireRate.style.display = DisplayStyle.Flex;
                    fieldBurst.style.display = DisplayStyle.Flex;
                    fieldsCharge.style.display = DisplayStyle.None;
                    break;
                    
                case ShootMode.FullAuto:
                    fieldAutoLoading.style.display = DisplayStyle.Flex;
                    fieldAutoLoadDuration.style.display = autoLoading.enumValueIndex != 0
                        ? DisplayStyle.Flex
                        : DisplayStyle.None;
                    fieldFireRate.style.display = DisplayStyle.Flex;
                    fieldBurst.style.display = DisplayStyle.None;
                    fieldsCharge.style.display = DisplayStyle.None;
                    break;
                    
                case ShootMode.Charge:
                    fieldAutoLoading.style.display = DisplayStyle.None;
                    fieldAutoLoadDuration.style.display = DisplayStyle.None;
                    fieldFireRate.style.display = DisplayStyle.None;
                    fieldBurst.style.display = DisplayStyle.None;
                    fieldsCharge.style.display = DisplayStyle.Flex;
                    break;
                    
                default: throw new ArgumentOutOfRangeException();
            }
            
            SerializedProperty fireAnimation = property.FindPropertyRelative("m_FireAnimation");
            SerializedProperty fireAvatarMask = property.FindPropertyRelative("m_FireAvatarMask");
            SerializedProperty fireTransitionIn = property.FindPropertyRelative("m_TransitionIn");
            SerializedProperty fireTransitionOut = property.FindPropertyRelative("m_TransitionOut");
            SerializedProperty fireRootMotion = property.FindPropertyRelative("m_RootMotion");
            SerializedProperty fireAudio = property.FindPropertyRelative("m_FireAudio");
            SerializedProperty emptyAudio = property.FindPropertyRelative("m_EmptyAudio");
            SerializedProperty loadStartAudio = property.FindPropertyRelative("m_LoadStartAudio");
            SerializedProperty loadLoopAudio = property.FindPropertyRelative("m_LoadLoopAudio");
            SerializedProperty loadMinPitch = property.FindPropertyRelative("m_LoadMinPitch");
            SerializedProperty loadMaxPitch = property.FindPropertyRelative("m_LoadMaxPitch");
            SerializedProperty muzzleEffect = property.FindPropertyRelative("m_MuzzleEffect");
            SerializedProperty force = property.FindPropertyRelative("m_Force");
            SerializedProperty power = property.FindPropertyRelative("m_Power");
            
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(fireAnimation));
            container.Add(new PropertyField(fireAvatarMask));
            container.Add(new PropertyField(fireTransitionIn));
            container.Add(new PropertyField(fireTransitionOut));
            container.Add(new PropertyField(fireRootMotion));
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(fireAudio));
            container.Add(new PropertyField(emptyAudio));
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(loadStartAudio));
            container.Add(new PropertyField(loadLoopAudio));
            container.Add(new PropertyField(loadMinPitch));
            container.Add(new PropertyField(loadMaxPitch));
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(muzzleEffect));
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(force));
            container.Add(new PropertyField(power));
        }
    }
}