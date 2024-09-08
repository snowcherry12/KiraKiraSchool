using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Stats;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Stats
{
    [CustomPropertyDrawer(typeof(StatusEffects))]
    public class StatusEffectsDrawer : TTitleDrawer
    {
        private const string PROP_STATUS_EFFECTS = "m_List";

        private const string CLASS_CONTROLS = "GC-Stats-StatusEffects-Controls";
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override string Title => "Status Effects";

        protected override string ExtraStyleSheetPath =>
            EditorPaths.PACKAGES + "Stats/Editor/StyleSheets/StatusEffects";

        // PAINT METHOD: --------------------------------------------------------------------------

        protected override void CreateContent(VisualElement body, SerializedProperty property)
        {
            SerializedProperty statusEffects = property.FindPropertyRelative(PROP_STATUS_EFFECTS);

            Button buttonAddEverything = new Button(() =>
            {
                string[] guids = AssetDatabase.FindAssets($"t:{nameof(StatusEffect)}");
                statusEffects.ClearArray();

                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    StatusEffect statusEffect = AssetDatabase.LoadAssetAtPath<StatusEffect>(path);
                    
                    statusEffects.InsertArrayElementAtIndex(0);
                    statusEffects.GetArrayElementAtIndex(0).objectReferenceValue = statusEffect;
                }
                
                SerializationUtils.ApplyUnregisteredSerialization(property.serializedObject);
            })
            {
                text = "Refresh Status Effects"
            };

            VisualElement controls = new VisualElement { name = CLASS_CONTROLS };
            controls.Add(buttonAddEverything);
            controls.Add(new FlexibleSpace());
            body.Add(controls);

            this.AddList(body, statusEffects);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private bool Contains(SerializedProperty statusEffects, StatusEffect statusEffect)
        {
            int statusEffectsSize = statusEffects.arraySize;
            for (int i = 0; i < statusEffectsSize; ++i)
            {
                SerializedProperty element = statusEffects.GetArrayElementAtIndex(i);
                if (element.objectReferenceValue == statusEffect) return true;
            }

            return false;
        }
        
        private void AddList(VisualElement body, SerializedProperty statusEffects)
        {
            int statusEffectsLength = statusEffects.arraySize;

            for (int i = 0; i < statusEffectsLength; ++i)
            {
                SerializedProperty statusEffect = statusEffects.GetArrayElementAtIndex(i);
                StatusEffect value = statusEffect.objectReferenceValue as StatusEffect;
                if (value == null) continue;

                PropertyField fieldStatusEffect = new PropertyField(
                    statusEffect,
                    value.ID.String
                );
                
                fieldStatusEffect.SetEnabled(false);
                body.Add(fieldStatusEffect);
            }
        }
    }
}