using GameCreator.Editor.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    [CustomPropertyDrawer(typeof(SkillEffects))]
    public class SkillEffectsDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Effects";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            SerializedProperty soundUse = property.FindPropertyRelative("m_SoundUse");
            SerializedProperty soundStrike = property.FindPropertyRelative("m_SoundStrike");
            SerializedProperty soundHit = property.FindPropertyRelative("m_SoundHit");
            SerializedProperty soundBlocked = property.FindPropertyRelative("m_SoundBlocked");
            SerializedProperty soundParried = property.FindPropertyRelative("m_SoundParried");
            SerializedProperty timeScale = property.FindPropertyRelative("m_TimeScale");
            
            PropertyField fieldSoundUse = new PropertyField(soundUse);
            PropertyField fieldSoundStrike = new PropertyField(soundStrike);
            PropertyField fieldSoundHit = new PropertyField(soundHit);
            PropertyField fieldSoundBlocked = new PropertyField(soundBlocked);
            PropertyField fieldSoundParried = new PropertyField(soundParried);
            PropertyField fieldTimeScale = new PropertyField(timeScale);
            
            container.Add(fieldSoundUse);
            container.Add(new SpaceSmallest());
            container.Add(fieldSoundStrike);
            container.Add(new SpaceSmallest());
            container.Add(fieldSoundHit);
            container.Add(new SpaceSmallest());
            container.Add(fieldSoundBlocked);
            container.Add(new SpaceSmallest());
            container.Add(fieldSoundParried);
            container.Add(new SpaceSmallest());
            container.Add(fieldTimeScale);
            
            SerializedProperty hitPause = property.FindPropertyRelative("m_HitPause");
            SerializedProperty hitPauseScale = property.FindPropertyRelative("m_HitPauseTimeScale");
            SerializedProperty hitPauseDelay = property.FindPropertyRelative("m_HitPauseDelay");
            SerializedProperty hitPauseDuration = property.FindPropertyRelative("m_HitPauseDuration");
            
            PropertyField fieldHitPause = new PropertyField(hitPause);
            PropertyField fieldHitPauseScale = new PropertyField(hitPauseScale, "Time Scale");
            PropertyField fieldHitPauseDelay = new PropertyField(hitPauseDelay, "Delay");
            PropertyField fieldHitPauseDuration = new PropertyField(hitPauseDuration, "Duration");
            
            VisualElement hitEffectContent = new VisualElement();
            
            container.Add(new SpaceSmall());
            container.Add(fieldHitPause);
            container.Add(new SpaceSmallest());
            container.Add(hitEffectContent);
            
            hitEffectContent.Add(fieldHitPauseScale);
            hitEffectContent.Add(fieldHitPauseDelay);
            hitEffectContent.Add(fieldHitPauseDuration);

            hitEffectContent.SetEnabled(hitPause.boolValue);
            fieldHitPause.RegisterValueChangeCallback(changeEvent =>
            {
                hitEffectContent.SetEnabled(changeEvent.changedProperty.boolValue);
            });
            
            SerializedProperty hitEffect = property.FindPropertyRelative("m_HitEffect");
            PropertyField fieldHitEffect = new PropertyField(hitEffect);
            
            container.Add(new SpaceSmall());
            container.Add(fieldHitEffect);
        }
    }
}